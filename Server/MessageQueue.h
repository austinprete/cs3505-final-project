/**
 * Mathew Beseris, Cindy Liao, Cole Perschon, and Austin Prete
 * CS3505 - Spring 2018
 */

#ifndef MESSAGE_QUEUE_H
#define MESSAGE_QUEUE_H

#include <vector>
#include <string>

class MessageQueue
{
public:
  MessageQueue();

  void AddMessage(std::string message);

  bool IsEmpty() const;

  std::string PopMessage();

private:
  std::vector<std::string> queue;
};


#endif //MESSAGE_QUEUE_H
