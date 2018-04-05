/**
 * Mathew Beseris, Cindy Liao, Cole Perschon, and Austin Prete
 * CS3505 - Spring 2018
 */

#include <cstdlib>
#include <istream>
#include <iostream>
#include <boost/asio.hpp>
#include <boost/algorithm/string.hpp>

#include "message_queue.h"
#include "session.h"

using boost::asio::ip::tcp;

using namespace std;

session::session(boost::asio::ip::tcp::socket socket, message_queue *queue)
    : socket(std::move(socket)), queue(queue)
{
}

void session::read_message()
{
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
          queue->add_message(message_string);

          write_message(length);
        }
      }
  );
}

void session::write_message(std::size_t length)
{
  if (!outbound_queue.is_empty()) {
    auto self(shared_from_this());
    boost::asio::async_write(
        socket,
        boost::asio::buffer(outbound_queue.pop_message()),
        [this, self](boost::system::error_code ec, std::size_t /*length*/) {
          if (!ec) {
            read_message();
          }
        }
    );
  } else {
    read_message();
  }
}

void session::start()
{
  read_message();
}

const string session::get_address() const
{
  return this->socket.remote_endpoint().address().to_string();
}

void session::add_message_to_outbound_queue(std::string message)
{
  outbound_queue.add_message(message);
  write_message(0);
}
