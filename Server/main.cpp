/**
 * Mathew Beseris, Cindy Liao, Cole Perschon, and Austin Prete
 * CS3505 - Spring 2018
 */

#include <cstdlib>
#include <iostream>
#include <thread>
#include <boost/asio.hpp>

#include "Server.h"

using boost::asio::ip::tcp;
using namespace std;

int main(int argc, char *argv[])
{
  try {
    if (argc != 2) {
      cerr << "Usage: server <port>\n";
      return 1;
    }

    boost::asio::io_service io_service;
    int port = atoi(argv[1]);

    Server spreadsheet_server(io_service, port);

    cout << "Running server on port " << port << endl;
    std::thread server_loop_thread(&Server::RunServerLoop, &spreadsheet_server);
    io_service.run();
    server_loop_thread.join();
  }
  catch (std::exception &e) {
    std::cerr << "Exception: " << e.what() << "\n";
  }

  return 0;
}