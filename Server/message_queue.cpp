/**
 * Mathew Beseris, Cindy Liao, Cole Perschon, and Austin Prete
 * CS3505 - Spring 2018
 */

#include <cstdlib>
#include "message_queue.h"

using namespace std;

void message_queue::add_message(std::string message)
{
  queue.push_back(message);
}

message_queue::message_queue()
    : queue()
{

}

std::string message_queue::pop_message()
{
  if (!queue.empty()) {
    string message = queue.at(0);
    queue.erase(queue.begin());
    return message;
  }

  return NULL;
}

bool message_queue::is_empty() const
{
  return queue.empty();
}
