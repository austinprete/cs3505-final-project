#include <cstdlib>
#include <iostream>
#include <boost/asio.hpp>

#include "session.h"

using boost::asio::ip::tcp;

session::session(tcp::socket socket)
    : socket(std::move(socket))
{
}

void session::read_message()
{
  auto self(shared_from_this());
  socket.async_read_some(
      boost::asio::buffer(data, max_length),
      [this, self](boost::system::error_code ec, std::size_t length) {
        if (!ec) {
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
      boost::asio::buffer(data, length),
      [this, self](boost::system::error_code ec, std::size_t /*length*/) {
        if (!ec) {
          read_message();
        }
        std::cout << "Message received: " << data << std::endl;
      }
  );
}

void session::start()
{
  read_message();
}