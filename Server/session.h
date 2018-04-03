/**
 * Mathew Beseris, Cindy Liao, Cole Perschon, and Austin Prete
 * CS3505 - Spring 2018
 */

#ifndef SESSION_H
#define SESSION_H

#include <cstdlib>
#include <iostream>
#include <boost/asio.hpp>

class session
    : public std::enable_shared_from_this<session>
{
public:
  session(boost::asio::ip::tcp::socket socket);

  void start();

private:
  void read_message();

  void write_message(std::size_t length);

  boost::asio::ip::tcp::socket socket;
  boost::asio::streambuf buffer;
  std::vector<std::string> message_queue;
};


#endif //SESSION_H
