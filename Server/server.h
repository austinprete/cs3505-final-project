/**
 * Mathew Beseris, Cindy Liao, Cole Perschon, and Austin Prete
 * CS3505 - Spring 2018
 */

#ifndef SERVER_H
#define SERVER_H

#include <cstdlib>
#include <iostream>
#include <boost/asio.hpp>


class server
{
public:
  server(boost::asio::io_service &io_service, int port);

private:
  void accept_connection();

  boost::asio::ip::tcp::acceptor acceptor;
  boost::asio::ip::tcp::socket socket;
};


#endif //SERVER_H
