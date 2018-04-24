/**
 * Mathew Beseris, Cindy Liao, Cole Perschon, and Austin Prete
 * CS3505 - Spring 2018
 */

#include <cstdlib>
#include <iostream>
#include <thread>
#include <boost/asio.hpp>
#include <boost/filesystem.hpp>

#include "Server.h"

using boost::asio::ip::tcp;
using namespace std;

void RunServer(boost::asio::io_service *service)
{
  service->run();
}

int main(int argc, char *argv[])
{
  const string spreadsheets_dir = "spreadsheets";

  if (boost::filesystem::exists(spreadsheets_dir)) {

  } else {
    boost::filesystem::create_directory(spreadsheets_dir);
    Spreadsheet::CreateSpreadsheetsMapXmlFile(spreadsheets_dir);
  }

  try {
    boost::asio::io_service io_service;
    int port = 2112;

    Server spreadsheet_server(io_service, port);

    Server::LogMessage("Running server on port " + to_string(port), false);

    std::thread server_loop_thread(&Server::RunServerLoop, &spreadsheet_server);

    std::thread server_thread(RunServer, &io_service);

    while (true) {
      string input;
      cin >> input;

      if (input == "quit") {
        spreadsheet_server.ShutdownServer();
        io_service.stop();
        server_loop_thread.join();
        server_thread.join();
        break;
      } else {
        Server::LogMessage("Unrecognized command: " + input, true);
      }
    }

  }
  catch (std::exception &e) {
    std::cerr << "Exception: " << e.what() << "\n";
  }

  return 0;
}
