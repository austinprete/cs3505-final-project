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
#include "message_queue.h"

#include "session.h"

class server
{
  friend class session;
public:
  server(boost::asio::io_service &io_service, int port);
  void run_server_loop();

private:
  std::vector<std::weak_ptr<session> > clients;

  void accept_connection();

  boost::asio::ip::tcp::acceptor acceptor;
  boost::asio::ip::tcp::socket socket;

  message_queue inbound_queue;

  bool process_message_in_queue();
  void process_message(std::string &message);
};

#endif //SERVER_H
