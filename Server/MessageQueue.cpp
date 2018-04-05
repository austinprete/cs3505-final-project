/**
 * Mathew Beseris, Cindy Liao, Cole Perschon, and Austin Prete
 * CS3505 - Spring 2018
 */

#include <cstdlib>
#include <thread>
#include <mutex>

#include "MessageQueue.h"

using namespace std;

MessageQueue::MessageQueue()
    : queue()
{}

void MessageQueue::AddMessage(std::string message)
{
  std::lock_guard<std::mutex> guard(queue_mutex);
  queue.push_back(message);
}

bool MessageQueue::IsEmpty() const
{
  return queue.empty();
}

std::string MessageQueue::PopMessage()
{
  std::lock_guard<std::mutex> guard(queue_mutex);

  if (!queue.empty()) {
    string message = queue.at(0);
    queue.erase(queue.begin());
    return message;
  }

  return nullptr;
}
