# Simple TCP client to test server with
# Run with: python test_tcp_client.py

import socket

host = raw_input("Enter hostname: ")

port = 2112  # The same port as used by the server
s = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
s.connect((host, port))

while True:
    message = raw_input('Message: ')

    if message == "exit":
        break

    s.sendall(message + b'\3')
    data = s.recv(1024)
    print("Received response: %s" % repr(data))
s.close()
