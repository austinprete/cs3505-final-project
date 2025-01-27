/**
 * Mathew Beseris, Cindy Liao, Cole Perschon, and Austin Prete
 * CS3505 - Spring 2018
 */

#ifndef SERVER_H
#define SERVER_H

#include <cstdlib>
#include <iostream>
#include <map>
#include <vector>
#include <boost/asio.hpp>

#include "MessageQueue.h"
#include "Session.h"
#include "Spreadsheet.h"

class Server
{
  friend class Session;

public:
  Server(boost::asio::io_service &io_service, int port);

  void RunServerLoop();

  void ShutdownServer();

  static void LogMessage(const std::string &message, bool is_error);

private:

  static long current_session_id;
  std::map<long, std::weak_ptr<Session> > clients;
  std::mutex clients_mutex;
  std::map<std::string, Spreadsheet *> spreadsheets;
  std::map<long, Spreadsheet *> open_spreadsheets_map;

  std::map<long, long> time_since_last_ping;
  boost::asio::ip::tcp::acceptor acceptor;

  boost::asio::ip::tcp::socket socket;

  MessageQueue inbound_queue;

  bool shutting_down = false;

  void AcceptConnection();

  void ProcessMessage(long client_id, std::string &message);

  bool ProcessMessageInQueue();

  void SendMessageToAllClients(std::string message) const;

  void SendMessageToAllSpreadsheetSubscribers(std::string sheet_name, std::string message) const;

  void SendMessageToClient(long client_id, std::string message) const;

  void RegisterClient(long client_id);

  void DisconnectClient(long client_id);

  void LoadSpreadsheet(long client_id, std::string spreadsheet_name);

  void RespondToPing(long client_id) const;

  void EditSpreadsheet(long client_id, std::string cell_id, std::string cell_contents);

  void RevertSpreadsheetCell(long client_id, std::string cell_id);

  void PingClient(int client_id);

  void HandlePingResponse(int client_id);

  void HandleFocusMessage(int client_id, std::string cell_id);

  void HandleUnfocusMessage(int client_id);

  void UndoLastChange(long client_id);
};

#endif //SERVER_H
