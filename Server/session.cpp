/**
 * Mathew Beseris, Cindy Liao, Cole Perschon, and Austin Prete
 * CS3505 - Spring 2018
 */

#include <cstdlib>
#include <istream>
#include <iostream>
#include <boost/asio.hpp>
#include <boost/algorithm/string.hpp>

#include "session.h"

using boost::asio::ip::tcp;

using namespace std;

session::session(tcp::socket socket, server &spreadsheet_server)
    : socket(std::move(socket)), spreadsheet_server(spreadsheet_server)
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

          message_queue.insert(message_queue.begin(), message_string);

          std::string::size_type pos = message_string.find('\3');
          if (pos != std::string::npos) {
            message_string = message_string.substr(0, pos);
          }

          std::cout << "Received message from " << client_address << ": " << message_string << std::endl;
          spreadsheet_server.add_message_to_queue(message_string);

          write_message(length);
        }
      }
  );
}

void session::write_message(std::size_t length)
{
  auto self(shared_from_this());
  boost::asio::async_write(
      socket,
      boost::asio::buffer(message_queue.back()),
      [this, self](boost::system::error_code ec, std::size_t /*length*/) {
        if (!ec) {
          message_queue.pop_back();
          read_message();
        }
      }
  );
}

void session::start()
{
  read_message();
}

const string session::get_address() const
{
  return this->socket.remote_endpoint().address().to_string();
}