/**
 * Mathew Beseris, Cindy Liao, Cole Perschon, and Austin Prete
 * CS3505 - Spring 2018
 */

#ifndef SERVER_H
#define SERVER_H

#include <cstdlib>
#include <iostream>
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
  std::vector<std::weak_ptr<Session> > clients;

  boost::asio::ip::tcp::acceptor acceptor;
  boost::asio::ip::tcp::socket socket;

  MessageQueue inbound_queue;

  void AcceptConnection();

  void ProcessMessage(std::string &message);

  bool ProcessMessageInQueue();

  void SendMessageToClients(std::string &message) const;
};

#endif //SERVER_H
