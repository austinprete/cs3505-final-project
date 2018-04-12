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

Session::Session(boost::asio::ip::tcp::socket socket, long session_id, MessageQueue *queue)
    : socket(std::move(socket)), inbound_queue(queue), id(session_id)
{
}

void Session::AddMessageToOutboundQueue(std::string message)
{
  outbound_queue.AddMessage(this->id, std::move(message));
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
  try {
    boost::asio::async_read_until(
        socket,
        buffer,
        '\3',
        [this, self](boost::system::error_code ec, std::size_t length) {

          if ((boost::asio::error::eof == ec) ||
              (boost::asio::error::connection_reset == ec)) {
            Shutdown(ec);
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

            std::cout << "Received message from " << GetAddress() << ": " << message_string << std::endl;
            inbound_queue->AddMessage(this->id, message_string);

            ReadMessage();
          }
        }
    );
  } catch (std::exception e) {
    cout << "Encountered exception" << endl;
  }
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
        boost::asio::buffer(outbound_queue.PopMessage().second),
        [this, self](boost::system::error_code ec, std::size_t /*length*/) {
          if ((boost::asio::error::eof == ec) ||
              (boost::asio::error::connection_reset == ec)) {

            Shutdown(ec);
            return;
          }
        }
    );
  }
}

bool Session::IsOpen() const
{
  return socket.is_open();
}

void Session::Shutdown(boost::system::error_code ec)
{
  cout << "Client at address " << GetAddress() << " disconnected" << endl;
  socket.shutdown(boost::asio::ip::tcp::socket::shutdown_receive, ec);
  socket.close();
}
