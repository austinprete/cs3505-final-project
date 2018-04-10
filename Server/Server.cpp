/**
 * Mathew Beseris, Cindy Liao, Cole Perschon, and Austin Prete
 * CS3505 - Spring 2018
 */

#include <cstdlib>
#include <iostream>
#include <vector>
#include <boost/asio.hpp>
#include <boost/algorithm/string.hpp>

#include "Session.h"
#include "Server.h"

using boost::asio::ip::tcp;
using namespace std;

long Server::current_session_id = 0;

Server::Server(boost::asio::io_service &io_service, int port)
    : acceptor(io_service, tcp::endpoint(tcp::v4(), port)),
      socket(io_service)
{
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
          std::cout << "Client connected from " << socket.remote_endpoint().address().to_string() << std::endl;

          shared_ptr<Session> session = std::make_shared<Session>(std::move(socket), current_session_id, (&inbound_queue));
          session->Start();
          clients.insert( std::pair<long, std::weak_ptr<Session> >(current_session_id, session));

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
    RegisterClient(client_id);
    cout << "Running register()" << endl;
  } else if (message_type == "disconnect") {
    cout << "Running disconnect()" << endl;
  } else if (message_type == "load") {
    cout << "Running load()" << endl;
  } else if (message_type == "ping") {
    cout << "Running ping()" << endl;
  } else if (message_type == "ping_response") {
    cout << "Running ping_response()" << endl;
  } else if (message_type == "edit") {
    cout << "Running edit()" << endl;
  } else if (message_type == "undo") {
    cout << "Running undo()" << endl;
  } else if (message_type == "revert") {
    cout << "Running revert()" << endl;
  } else {
    cout << "ERROR: Received unrecognized message type \"" << message_type << "\"" << endl;
  }

  std::string response_message = "Server received message: " + message;
  SendMessageToClient(client_id, response_message);
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

  for (auto it = clients.begin(); it != clients.end(); ++it) {
    weak_ptr<Session> session = it->second;

    if (auto spt = session.lock()) { // Has to be copied into a shared_ptr before usage
      if ((*spt).IsOpen()) {
        (*spt).AddMessageToOutboundQueue(message);
      }
    }
  }
}

void Server::SendMessageToClient(long client_id, string message) const
{
  message.append(" ");
  message += '\3';

  auto search = clients.find(client_id);

  if(search != clients.end()) {
    weak_ptr<Session> session = search->second;

    if (auto spt = session.lock()) { // Has to be copied into a shared_ptr before usage
      if ((*spt).IsOpen()) {
        (*spt).AddMessageToOutboundQueue(message);
      }
    }
  }
}

void Server::RegisterClient(long client_id)
{
  string accept_message = "connect_accepted ";
  SendMessageToClient(client_id, accept_message);
}
