#include <cstdlib>
#include <iostream>
#include <boost/asio.hpp>

#include "server.h"

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

    server spreadsheet_server(io_service, port);

    cout << "Running server on port " << port << endl;
    io_service.run();
  }
  catch (std::exception &e) {
    std::cerr << "Exception: " << e.what() << "\n";
  }

  return 0;
}