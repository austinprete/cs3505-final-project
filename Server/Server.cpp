/**
 * Mathew Beseris, Cindy Liao, Cole Perschon, and Austin Prete
 * CS3505 - Spring 2018
 */

#include <algorithm>
#include <iostream>
#include <vector>
#include <map>
#include <thread>
#include <boost/asio.hpp>
#include <boost/algorithm/string.hpp>
#include <boost/filesystem.hpp>
#include <boost/regex.hpp>

#include "Session.h"
#include "Server.h"
#include "Spreadsheet.h"

using boost::asio::ip::tcp;
using namespace std;

long Server::current_session_id = 0;
const std::string spreadsheets_directory = "spreadsheets";

Server::Server(boost::asio::io_service &io_service, int port)
    : acceptor(io_service, tcp::endpoint(tcp::v4(), port)),
      socket(boost::asio::ip::tcp::socket(io_service))
{
  spreadsheets = Spreadsheet::LoadSpreadsheetsMapFromXml(spreadsheets_directory);

  AcceptConnection();
}

void Server::RunServerLoop()
{
  while (true) {
    bool idle = true;

    // Returns true if there are still messages in the queue
    if (ProcessMessageInQueue()) {
      idle = false;
    };

    if (idle) {
      sleep(1);
    }
  }
}

void Server::AcceptConnection()
{
  acceptor.async_accept(
      socket,
      [this](boost::system::error_code ec) {
        if (!ec) {
          std::cout << "Client " << current_session_id << " connected from "
                    << socket.remote_endpoint().address().to_string() << std::endl;

          shared_ptr<Session> session = std::make_shared<Session>(std::move(socket), current_session_id,
                                                                  (&inbound_queue));


          session->Start();

          std::lock_guard<std::mutex> guard(clients_mutex);
          clients.emplace(std::make_pair(current_session_id, session));
          current_session_id++;
        }

        AcceptConnection();
      }
  );
}

void Server::ProcessMessage(long client_id, string &message)
{
  vector<string> tokenized_message;
  split(tokenized_message, message, boost::is_any_of(" \t"), boost::token_compress_on);

  string message_type = tokenized_message.at(0);

  if (message_type == "register") {
    cout << "Running RegisterClient()" << endl;
    RegisterClient(client_id);
  } else if (message_type == "disconnect") {
    cout << "Running DisconnectClient()" << endl;
    DisconnectClient(client_id);
  } else if (message_type == "load") {
    cout << "Running LoadSpreadsheet()" << endl;
    string spreadsheet_name = boost::replace_all_copy(message, "load ", "");
    if (spreadsheet_name.length() != 0) {
      LoadSpreadsheet(client_id, spreadsheet_name);
    }
  } else if (message_type == "ping") {
    cout << "Running RespondToPing()" << endl;
    RespondToPing(client_id);
  } else if (message_type == "ping_response") {
    cout << "Running HandlePingResponse()" << endl;

    HandlePingResponse(client_id);
  } else if (message_type == "edit") {
    cout << "Running EditSpreadsheet()" << endl;
    string edit_info = boost::replace_all_copy(message, "edit ", "");
    vector<string> tokenized_edit_info;
    split(tokenized_edit_info, edit_info, boost::is_any_of(":"), boost::token_compress_on);

    if (tokenized_edit_info.size() == 2) {
      EditSpreadsheet(client_id, tokenized_edit_info.at(0), tokenized_edit_info.at(1));
    }
  } else if (message_type == "undo") {
    cout << "Running undo()" << endl;
  } else if (message_type == "revert") {
    cout << "Running RevertSpreadsheetCell()" << endl;

    if (tokenized_message.size() == 2) {
      RevertSpreadsheetCell(client_id, tokenized_message.at(1));
    }
  } else if (message_type == "focus") {
    cout << "Running HandleFocusMessage()" << endl;

    if (tokenized_message.size() == 2) {
      HandleFocusMessage(client_id, tokenized_message.at(1));
    }
  } else if (message_type == "unfocus") {
    cout << "Running HandleUnfocusMessage()" << endl;

    if (message == "unfocus ") {
      HandleUnfocusMessage(client_id);
    }

  } else {
    cout << "ERROR: Received unrecognized message type \"" << message_type << "\"" << endl;
  }

}

/**
 * Processes a single message from the queue
 * @return true if there are still messages to process (queue not empty), false otherwise
 */
bool Server::ProcessMessageInQueue()
{
  if (!inbound_queue.IsEmpty()) {
    std::pair<long, string> message_pair = inbound_queue.PopMessage();

    long client_id = message_pair.first;
    string message = message_pair.second;

    ProcessMessage(client_id, message);
  }

  return !inbound_queue.IsEmpty();
}

void Server::SendMessageToAllClients(string message) const
{
  message.append(" ");
  message += '\3';

  for (const auto &client : clients) {
    weak_ptr<Session> session = client.second;

    if (auto spt = session.lock()) { // Has to be copied into a shared_ptr before usage
//      if ((*spt).IsOpen()) {
      (*spt).AddMessageToOutboundQueue(message);
//      }
    }
  }
}

void Server::SendMessageToClient(long client_id, string message) const
{
  message.append(" ");
  message += '\3';

  auto search = clients.find(client_id);

  if (search != clients.end()) {
    weak_ptr<Session> session = search->second;

    if (auto spt = session.lock()) {
      (*spt).AddMessageToOutboundQueue(message);
    }
  }
}

void Server::RegisterClient(long client_id)
{
  // temporarily hardcoding example message

  string accept_message = "connect_accepted ";

  for (auto spreadsheet : spreadsheets) {
    accept_message.append(spreadsheet.first);
    accept_message.append("\n");
  }

  SendMessageToClient(client_id, accept_message);
}

void Server::LoadSpreadsheet(long client_id, string spreadsheet_name)
{
  string response;

  auto search = spreadsheets.find(spreadsheet_name);

  string spreadsheet_file_name = boost::regex_replace(spreadsheet_name, boost::regex("\\s+"), "_");
  spreadsheet_file_name.append(".xml");

  if (search != spreadsheets.end()) {
    Spreadsheet *sheet = search->second;

    open_spreadsheets_map.insert(std::make_pair(client_id, sheet));
    sheet->AddSubscriber(client_id);

    response = sheet->GetFullStateString();
  } else {
    Spreadsheet *sheet = new Spreadsheet(spreadsheet_name, spreadsheet_file_name);

    sheet->WriteSpreadsheetToFile(spreadsheets_directory);

    spreadsheets.insert(std::make_pair(spreadsheet_name, sheet));
    open_spreadsheets_map.insert(std::make_pair(client_id, sheet));
    sheet->AddSubscriber(client_id);

    Spreadsheet::WriteSpreadsheetsMapXmlFile(spreadsheets_directory, (&spreadsheets));
    response = sheet->GetFullStateString();
  }

  SendMessageToClient(client_id, response);

  auto *client_loop_thread = new thread(&Server::PingClient, this, client_id);
}

void Server::DisconnectClient(long client_id)
{
  cout << "Disconnected client " << client_id << endl;

  auto open_sheet_search = open_spreadsheets_map.find(client_id);

  if (open_sheet_search != open_spreadsheets_map.end()) {
    open_sheet_search->second->RemoveSubscriber(client_id);
  }

  open_spreadsheets_map.erase(client_id);

  auto session_search = clients.find(client_id);

  if (session_search != clients.end()) {
    auto session = session_search->second;

    if (auto spt = session.lock()) {
      if ((*spt).IsOpen()) {
        (*spt).Close();
      }
    }
  }

  std::lock_guard<std::mutex> guard(clients_mutex);
  clients.erase(client_id);
}

void Server::RespondToPing(long client_id) const
{
  SendMessageToClient(client_id, "ping_response ");
}

void Server::EditSpreadsheet(long client_id, string cell_id, string cell_contents)
{
  auto spreadsheet_search = open_spreadsheets_map.find(client_id);

  if (spreadsheet_search != open_spreadsheets_map.end()) {
    auto spreadsheet = spreadsheet_search->second;

    spreadsheet->ChangeCellContents(cell_id, cell_contents);
    spreadsheet->WriteSpreadsheetToFile(spreadsheets_directory);

    SendMessageToAllSpreadsheetSubscribers(spreadsheet->GetName(), "change " + cell_id + ":" + cell_contents);
  }
}

void Server::SendMessageToAllSpreadsheetSubscribers(std::string sheet_name, std::string message) const
{
  auto spreadsheet_search = spreadsheets.find(sheet_name);

  if (spreadsheet_search != spreadsheets.end()) {
    auto spreadsheet = spreadsheet_search->second;

    set<int> subscribing_clients = spreadsheet->GetSubscribers();

    for (auto client_id : subscribing_clients) {
      SendMessageToClient(client_id, message);
    }
  }
}

void Server::RevertSpreadsheetCell(long client_id, std::string cell_id)
{
  auto spreadsheet_search = open_spreadsheets_map.find(client_id);

  if (spreadsheet_search != open_spreadsheets_map.end()) {
    auto spreadsheet = spreadsheet_search->second;

    string new_value = spreadsheet->RevertCellContents(cell_id);
    spreadsheet->WriteSpreadsheetToFile(spreadsheets_directory);

    SendMessageToAllSpreadsheetSubscribers(spreadsheet->GetName(), "change " + cell_id + ":" + new_value);
  }
}

void Server::PingClient(int client_id)
{
  string message = "ping";

  while (true) {
    SendMessageToClient(client_id, message);

    auto search = time_since_last_ping.find(client_id);

    if (search == time_since_last_ping.end()) {
      time_since_last_ping.insert(make_pair(client_id, 0));
      search = time_since_last_ping.find(client_id);
    }

    sleep(10);

    search->second += 10;

    if (search->second > 60) {
      SendMessageToClient(client_id, "disconnect ");
      DisconnectClient(client_id);
      return;
    }
  }
}

void Server::HandlePingResponse(int client_id)
{
  auto search = time_since_last_ping.find(client_id);

  if (search == time_since_last_ping.end()) {
    time_since_last_ping.insert(make_pair(client_id, 0));
    search = time_since_last_ping.find(client_id);
  }

  cout << "resetting ping time for client " << client_id << endl;

  search->second = 0;
}

void Server::HandleFocusMessage(int client_id, std::string cell_id)
{
  auto search = clients.find(client_id);

  if (search == clients.end()) {
    return;
  }

  weak_ptr<Session> session = search->second;

  if (auto spt = session.lock()) {
    if ((*spt).IsFocused()) {
      return;
    }
  }

  string message = "focus " + cell_id + ":" + "client_" + to_string(client_id);

  auto spreadsheet_search = open_spreadsheets_map.find(client_id);

  if (spreadsheet_search != open_spreadsheets_map.end()) {
    auto spreadsheet = spreadsheet_search->second;
    SendMessageToAllSpreadsheetSubscribers(spreadsheet->GetName(), message);
  }

  if (auto spt = session.lock()) {
    (*spt).Focus();
  }
}

void Server::HandleUnfocusMessage(int client_id)
{
  auto search = clients.find(client_id);

  if (search == clients.end()) {
    return;
  }

  weak_ptr<Session> session = search->second;

  if (auto spt = session.lock()) {
    if (!(*spt).IsFocused()) {
      return;
    }
  }

  string message = "unfocus client_" + to_string(client_id);

  auto spreadsheet_search = open_spreadsheets_map.find(client_id);

  if (spreadsheet_search != open_spreadsheets_map.end()) {
    auto spreadsheet = spreadsheet_search->second;
    SendMessageToAllSpreadsheetSubscribers(spreadsheet->GetName(), message);
  }

  if (auto spt = session.lock()) {
    (*spt).Unfocus();
  }
}
