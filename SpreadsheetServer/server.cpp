//
//  server.cpp
//  3505SpreadsheetServer
//

#include "server.h"

server::server(){
    //Create socket
    int sockfd = socket(AF_INET, SOCK_STREAM, 0);
    //int setsockopt(int sockfd, int level, int optname,const void *optval, socklen_t optlen);
    sockaddr address;
    int bind(int sockfd, const struct sockaddr *addr, socklen_t addrlen);
    int listen(int sockfd, int backlog);
    int new_socket= accept(int sockfd, struct sockaddr *addr, socklen_t *addrlen);
    
    
}
