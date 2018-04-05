/**
 * Mathew Beseris, Cindy Liao, Cole Perschon, and Austin Prete
 * CS3505 - Spring 2018
 */

#include <cstdlib>
#include <istream>
#include <iostream>
#include <boost/asio.hpp>
#include <boost/algorithm/string.hpp>
#include <utility>

#include "MessageQueue.h"
#include "Session.h"

using boost::asio::ip::tcp;

using namespace std;

Session::Session(boost::asio::ip::tcp::socket socket, MessageQueue *queue)
    : socket(std::move(socket)), inbound_queue(queue)
{
}

void Session::AddMessageToOutboundQueue(std::string message)
{
  outbound_queue.AddMessage(std::move(message));
  WriteOutboundMessage();
}

const string Session::GetAddress() const
{
  return this->socket.remote_endpoint().address().to_string();
}

void Session::Start()
{
  ReadMessage();
}

void Session::ReadMessage()
{
  if (!IsOpen()) {
    return;
  }

  auto self(shared_from_this());
  boost::asio::async_read_until(
      socket,
      buffer,
      '\3',
      [this, self](boost::system::error_code ec, std::size_t length) {

        string client_address = socket.remote_endpoint().address().to_string();

        if ((boost::asio::error::eof == ec) ||
            (boost::asio::error::connection_reset == ec)) {
          cout << "Client at address " << client_address << " disconnected" << endl;
          return;
        }

        if (!ec) {

          std::string message_string;
          std::istream buffer_stream(&buffer);
          std::getline(buffer_stream, message_string);

          std::string::size_type pos = message_string.find('\3');
          if (pos != std::string::npos) {
            message_string = message_string.substr(0, pos);
          }

          std::cout << "Received message from " << client_address << ": " << message_string << std::endl;
          inbound_queue->AddMessage(message_string);

          ReadMessage();
        }
      }
  );
}

void Session::WriteOutboundMessage()
{
  if (!IsOpen()) {
    return;
  }
  if (!outbound_queue.IsEmpty()) {
    auto self(shared_from_this());
    boost::asio::async_write(
        socket,
        boost::asio::buffer(outbound_queue.PopMessage()),
        [this, self](boost::system::error_code ec, std::size_t /*length*/) {
          if (!ec) {
          }
        }
    );
  }
}

bool Session::IsOpen() const
{
  return socket.is_open();
}
