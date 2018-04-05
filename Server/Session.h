/**
 * Mathew Beseris, Cindy Liao, Cole Perschon, and Austin Prete
 * CS3505 - Spring 2018
 */

#ifndef SESSION_H
#define SESSION_H

#include <cstdlib>
#include <iostream>
#include <boost/asio.hpp>

#include "MessageQueue.h"

class Session
    : public std::enable_shared_from_this<Session>
{
public:
  Session(boost::asio::ip::tcp::socket socket, MessageQueue *queue);

  void AddMessageToOutboundQueue(std::string message);

  const std::string GetAddress() const;

  void Start();

private:
  boost::asio::ip::tcp::socket socket;

  boost::asio::streambuf buffer;
  MessageQueue *inbound_queue;

  MessageQueue outbound_queue;

  void ReadMessage();

  void WriteMessage(std::size_t length);
};


#endif //SESSION_H
