cmake_minimum_required(VERSION 3.9)
project(Server)

set(CMAKE_CXX_STANDARD 11)

find_package(Boost 1.66.0 COMPONENTS system filesystem regex date_time REQUIRED)
include_directories(${Boost_INCLUDE_DIRS})
add_executable(Server main.cpp Server.cpp Server.h Session.cpp Session.h MessageQueue.cpp MessageQueue.h Spreadsheet.cpp Spreadsheet.h)
target_link_libraries(Server ${Boost_LIBRARIES})
