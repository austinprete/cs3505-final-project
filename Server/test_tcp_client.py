# Simple TCP client to test server with
# Run with: python test_tcp_client.py

import socket
import sys

import time

host = "localhost"

if len(sys.argv) == 2:
    host = sys.argv[1]


#
# port = 2112  # The same port as used by the server
# s = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
# s.connect((host, port))
#
# while True:
#     message = raw_input('Message: ')
#
#     if message == "exit":
#         sys.exit(-1)
#         break
#
#     s.sendall(message + b'\3')
#
#     time.sleep(1)
#     data = s.recv(1024)
#     message_parts = data.split('\x03')
#
#     message_parts = filter(lambda x: x, message_parts)
#
#     for part in message_parts:
#         print("Received response: %s" % part)
# s.close()

def send_message(message, socket):
    socket.sendall(message + b'\3')
    time.sleep(1)


for i in range(1000):
    port = 2112  # The same port as used by the server
    s = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
    s.connect((host, port))

    send_message("register ", s)
    send_message("load test", s)

    s.close()
