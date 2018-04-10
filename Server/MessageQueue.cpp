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

void MessageQueue::AddMessage(long client_id, std::string message)
{
  std::lock_guard<std::mutex> guard(queue_mutex);
  queue.push_back(std::make_pair(client_id, message));
}

bool MessageQueue::IsEmpty() const
{
  return queue.empty();
}

pair<long, string> MessageQueue::PopMessage()
{
  std::lock_guard<std::mutex> guard(queue_mutex);

  if (!queue.empty()) {
    pair<long, string> message_pair = queue.at(0);
    queue.erase(queue.begin());
    return message_pair;
  }

  return std::make_pair<long, string>(NULL, NULL);
}
