/**
 * Mathew Beseris, Cindy Liao, Cole Perschon, and Austin Prete
 * CS3505 - Spring 2018
 */

#ifndef MESSAGE_QUEUE_H
#define MESSAGE_QUEUE_H

#include <mutex>
#include <string>
#include <vector>

class MessageQueue
{
public:
  MessageQueue();

  void AddMessage(long client_id, std::string message);

  bool IsEmpty() const;

  std::pair<long, std::string> PopMessage();

private:
  std::vector<std::pair<long, std::string> > queue;
  std::mutex queue_mutex;
};


#endif //MESSAGE_QUEUE_H
