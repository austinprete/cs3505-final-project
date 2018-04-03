a.out: server.o session.o
	g++ -std=c++11 main.cpp *.o -lpthread -l:libboost_system.so.1.53.0

server.o: server.h server.cpp server.cpp
	g++ -std=c++11 -g -c server.cpp

session.o: session.h session.cpp
	g++ -std=c++11 -g -c session.cpp

clean:
	rm *.o a.out
