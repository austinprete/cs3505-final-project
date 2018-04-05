/**
 * Mathew Beseris, Cindy Liao, Cole Perschon, and Austin Prete
 * CS3505 - Spring 2018
 */

#ifndef MESSAGE_QUEUE_H
#define MESSAGE_QUEUE_H

#include <vector>
#include <string>

class message_queue
{
public:
  void add_message(std::string);
  message_queue();

  std::string pop_message();

  bool is_empty() const;

private:
  std::vector<std::string> queue;
};


#endif //MESSAGE_QUEUE_H
