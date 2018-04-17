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
  Session(std::shared_ptr <boost::asio::ip::tcp::socket> socket, long session_id, MessageQueue *queue);

  void AddMessageToOutboundQueue(std::string message);

  const std::string GetAddress() const;

//  bool IsOpen() const;

  void Start();

//  void Close();

private:
  std::shared_ptr<boost::asio::ip::tcp::socket> socket;
  long id;

  boost::asio::streambuf buffer;

  MessageQueue *inbound_queue;

  MessageQueue outbound_queue;

  void ReadMessage();

  void Shutdown(boost::system::error_code ec);

  void WriteOutboundMessage();
};


#endif //SESSION_H
