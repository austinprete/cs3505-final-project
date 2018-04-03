//
//  server.hpp
//  3505SpreadsheetServer
//
#include <string>
#include <stack>
#include <sys/socket.h>

#ifndef server_h
#define server_h

#include <stdio.h>

class server{
    private:
    std::stack<std::string> inboundMessages;
    std::stack<std::string> outboundMessages;
    std::vector<socket?> connectedClients;
    
    public:
        server();
};

#endif /* server_hpp */
