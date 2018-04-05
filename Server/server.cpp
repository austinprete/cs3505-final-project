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
    bool idle = true;

    // Returns true if there are still messages in the queue
    if (process_message_in_queue()) {
      idle = false;
    };

//

    if (idle) {
      sleep(1);
    }
  }
}

void server::accept_connection()
{
  acceptor.async_accept(
      socket,
      [this](boost::system::error_code ec) {
        if (!ec) {
          std::cout << "Client connected from " << socket.remote_endpoint().address().to_string() << std::endl;

          shared_ptr<session> sesh = std::make_shared<session>(std::move(socket), (&inbound_queue));
          sesh->start();
          clients.push_back(sesh);
        }

        accept_connection();
      }
  );
}

/**
 * Processes a single message from the queue
 * @return true if there are still messages to process (queue not empty), false otherwise
 */
bool server::process_message_in_queue()
{
  if (!inbound_queue.is_empty()) {
    string message = inbound_queue.pop_message();
    process_message(message);
  }

  return !inbound_queue.is_empty();
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

  for (auto session : clients) {
    if (auto spt = session.lock()) { // Has to be copied into a shared_ptr before usage
      (*spt).add_message_to_outbound_queue("Response message");
    }
  }
}