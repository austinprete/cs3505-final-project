#include <cstdlib>
#include <iostream>
#include <boost/asio.hpp>

#include "session.h"
#include "server.h"

using boost::asio::ip::tcp;

server::server(boost::asio::io_service &io_service, int port)
    : acceptor(io_service, tcp::endpoint(tcp::v4(), port)),
      socket(io_service)
{
  accept_connection();
}

void server::accept_connection()
{
  acceptor.async_accept(
      socket,
      [this](boost::system::error_code ec) {
        if (!ec) {
          std::cout << "Client connected from " << socket.remote_endpoint().address().to_string() << std::endl;

          std::make_shared<session>(std::move(socket))->start();
        }

        accept_connection();
      }
  );
}