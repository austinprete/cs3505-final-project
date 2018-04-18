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

#include "Dependencies/rapidxml-1.13/rapidxml_utils.hpp"
#include "Spreadsheet.h"

using boost::asio::ip::tcp;
using namespace std;

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