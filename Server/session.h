/**
 * Mathew Beseris, Cindy Liao, Cole Perschon, and Austin Prete
 * CS3505 - Spring 2018
 */

#ifndef SESSION_H
#define SESSION_H

#include <cstdlib>
#include <iostream>
#include <boost/asio.hpp>
#include "message_queue.h"

class session
    : public std::enable_shared_from_this<session>
{
public:
  session(boost::asio::ip::tcp::socket socket, message_queue *queue);

  void start();

  void add_message_to_outbound_queue(std::string message);

private:
  void read_message();

  void write_message(std::size_t length);

  boost::asio::ip::tcp::socket socket;
  boost::asio::streambuf buffer;
  message_queue outbound_queue;

  message_queue * queue;

  const std::string get_address() const;
};


#endif //SESSION_H
