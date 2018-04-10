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

class Server
{
  friend class Session;

public:
  Server(boost::asio::io_service &io_service, int port);

  void RunServerLoop();

private:
  static long current_session_id;

  std::map<long, std::weak_ptr<Session> > clients;

  boost::asio::ip::tcp::acceptor acceptor;
  boost::asio::ip::tcp::socket socket;

  MessageQueue inbound_queue;

  void AcceptConnection();

  void ProcessMessage(long client_id, std::string &message);

  bool ProcessMessageInQueue();

  void SendMessageToAllClients(std::string message) const;

  void SendMessageToClient(long client_id, std::string message) const;

  void RegisterClient(long client_id);
};

#endif //SERVER_H
