/**
 * Mathew Beseris, Cindy Liao, Cole Perschon, and Austin Prete
 * CS3505 - Spring 2018
 */

#include <cstdlib>
#include <iostream>
#include <vector>
#include <boost/asio.hpp>
#include <boost/algorithm/string.hpp>

#include "session.h"
#include "server.h"

using boost::asio::ip::tcp;
using namespace std;

server::server(boost::asio::io_service &io_service, int port)
    : acceptor(io_service, tcp::endpoint(tcp::v4(), port)),
      socket(io_service)
{
  accept_connection();
}

void server::run_server_loop()
{
  while (true) {
    sleep(1);
    process_messages_in_queue();
  }
}

void server::accept_connection()
{
  acceptor.async_accept(
      socket,
      [this](boost::system::error_code ec) {
        if (!ec) {
          std::cout << "Client connected from " << socket.remote_endpoint().address().to_string() << std::endl;

          std::make_shared<session>(std::move(socket), (*this))->start();
        }

        accept_connection();
      }
  );
}

void server::add_message_to_queue(const std::string &message)
{
  message_queue.push_back(message);
}

void server::process_messages_in_queue()
{
  for (int index = 0; index < message_queue.size(); index++) {
    string message = message_queue.at(0);
    process_message(message);
    message_queue.erase(message_queue.begin());
  }
}

void server::process_message(string &message)
{
  vector<string> tokenized_message;
  split(tokenized_message, message, boost::is_any_of(" \t"), boost::token_compress_on);

  string message_type = tokenized_message.at(0);

  if (message_type == "register") {
    cout << "Running register()" << endl;
  } else if (message_type == "disconnect") {
    cout << "Running disconnect()" << endl;
  } else if (message_type == "load") {
    cout << "Running load()" << endl;
  } else if (message_type == "ping") {
    cout << "Running ping()" << endl;
  } else if (message_type == "ping_response") {
    cout << "Running ping_response()" << endl;
  } else if (message_type == "edit") {
    cout << "Running edit()" << endl;
  } else if (message_type == "undo") {
    cout << "Running undo()" << endl;
  } else if (message_type == "revert") {
    cout << "Running revert()" << endl;
  } else {
    cout << "ERROR: Received unrecognized message type \"" << message_type << "\"" << endl;
  }
}