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

session::session(tcp::socket socket)
    : socket(std::move(socket))
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

          std::string data_str;
          std::istream buffer_stream(&buffer);
          std::getline(buffer_stream, data_str);

          message_queue.insert(message_queue.begin(), data_str);

          std::string::size_type pos = data_str.find('\3');
          if (pos != std::string::npos)
            data_str = data_str.substr(0, pos);

          std::cout << "Received message from " << client_address << ": " << data_str << std::endl;

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