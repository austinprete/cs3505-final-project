#include <cstdlib>
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
  socket.async_read_some(
      boost::asio::buffer(data, max_length),
      [this, self](boost::system::error_code ec, std::size_t length) {

        if (!ec) {
          std::string data_str(data, length);

          std::string::size_type pos = data_str.find('\3');
          if (pos!=std::string::npos)
            data_str = data_str.substr(0, pos);

          std::cout << "Received message: " << data_str << std::endl;

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
      }
  );
}

void session::start()
{
  read_message();
}