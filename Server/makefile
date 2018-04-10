a.out: Server.o Session.o MessageQueue.o Spreadsheet.o
	g++ -std=c++11 -g main.cpp *.o -lpthread -l:libboost_system.so.1.53.0

Server.o: Server.h Server.cpp Server.cpp MessageQueue.h Spreadsheet.h
	g++ -std=c++11 -g -c Server.cpp

Session.o: Session.h Session.cpp MessageQueue.h
	g++ -std=c++11 -g -c Session.cpp

MessageQueue.o: MessageQueue.h MessageQueue.cpp
	g++ -std=c++11 -g -c MessageQueue.cpp

Spreadsheet.o: Spreadsheet.h Spreadsheet.cpp
	g++ -std=c++11 -g -c Spreadsheet.cpp

clean:
	rm *.o a.out
