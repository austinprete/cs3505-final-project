a.out: Server.o Session.o MessageQueue.o
	g++ -std=c++11 main.cpp *.o -lpthread -l:libboost_system.so.1.53.0

Server.o: Server.h Server.cpp Server.cpp MessageQueue.h
	g++ -std=c++11 -g -c Server.cpp

Session.o: Session.h Session.cpp MessageQueue.h
	g++ -std=c++11 -g -c Session.cpp

MessageQueue.o: MessageQueue.h MessageQueue.cpp
	g++ -std=c++11 -g -c MessageQueue.cpp

clean:
	rm *.o a.out
