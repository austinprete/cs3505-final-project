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


class server
{
  friend class session;
public:
  server(boost::asio::io_service &io_service, int port);
  void run_server_loop();

private:
  void accept_connection();

  boost::asio::ip::tcp::acceptor acceptor;
  boost::asio::ip::tcp::socket socket;

  std::vector<std::string> message_queue;
  void add_message_to_queue(const std::string &message);

  void process_messages_in_queue();
  void process_message(std::string &message);
};

#endif //SERVER_H
