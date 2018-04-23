using Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using static Network.Networking;

namespace Network
{
    /// <summary>
    /// This class holds all the necessary state to represent a socket connection
    /// Note that all of its fields are public because we are using it like a "struct"
    /// It is a simple collection of fields
    /// </summary>
    public class SocketState
    {
        public Socket theSocket;
        public int ID;
        public NetworkAction callMe;

        // This is the buffer where we will receive data from the socket
        public byte[] messageBuffer = new byte[1024];

        // This is a larger (growable) buffer, in case a single receive does not contain the full message.
        public StringBuilder sb = new StringBuilder();

        public SocketState(Socket s, int id)
        {
            theSocket = s;
            ID = id;
        }
    }

    /// <summary>
    /// ConnectionState creates objects that contain all of the information regarding a network server connection
    /// </summary>
    public class ConnectionState
    {
        public TcpListener listener;
        public NetworkAction callMe;

        public ConnectionState(TcpListener tcpListener, NetworkAction callBack)
        {
            listener = tcpListener;
            callMe = callBack;
        }
    }

    public class Networking
    {

        public const int DEFAULT_PORT = 2112;

        //private static SocketState state;
        public delegate void NetworkAction(SocketState socketState);

        public static Dictionary<int, Socket> connectedSockets = new Dictionary<int, Socket>();

        /// <summary>
        /// Creates a Socket object for the given host string
        /// </summary>
        /// <param name="hostName">The host name or IP address</param>
        /// <param name="socket">The created Socket</param>
        /// <param name="ipAddress">The created IPAddress</param>
        public static void MakeSocket(string hostName, out Socket socket, out IPAddress ipAddress)
        {
            ipAddress = IPAddress.None;
            socket = null;
            try
            {
                // Establish the remote endpoint for the socket.
                IPHostEntry ipHostInfo;

                // Determine if the server address is a URL or an IP
                try
                {
                    ipHostInfo = Dns.GetHostEntry(hostName);
                    bool foundIPV4 = false;
                    foreach (IPAddress addr in ipHostInfo.AddressList)
                        if (addr.AddressFamily != AddressFamily.InterNetworkV6)
                        {
                            foundIPV4 = true;
                            ipAddress = addr;
                            break;
                        }
                    // Didn't find any IPV4 addresses
                    if (!foundIPV4)
                    {
                        System.Diagnostics.Debug.WriteLine("Invalid addres: " + hostName);
                        throw new ArgumentException("Invalid address");
                    }
                }
                catch (Exception)
                {
                    // see if host name is actually an ipaddress, i.e., 155.99.123.456
                    System.Diagnostics.Debug.WriteLine("using IP");
                    ipAddress = IPAddress.Parse(hostName);
                }

                // Create a TCP/IP socket.
                socket = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

                socket.SetSocketOption(SocketOptionLevel.IPv6, SocketOptionName.IPv6Only, false);

                // Disable Nagle's algorithm - can speed things up for tiny messages, 
                // such as for a game
                socket.NoDelay = true;

            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine("Unable to create socket. Error occured: " + e);
                MessageBox.Show("Unable to create socket. Error occured: " + e.GetType());
                socket = null;
            }

        }


        /// <summary>
        /// Start attempting to connect to the server
        /// </summary>
        /// <param name="hostName"> server to connect to </param>
        /// <returns></returns>
        public static Socket ConnectToServer(NetworkAction callMe, string hostName)
        {
            System.Diagnostics.Debug.WriteLine("connecting  to " + hostName);

            // Create a TCP/IP socket.
            Socket socket;
            IPAddress ipAddress;

            MakeSocket(hostName, out socket, out ipAddress);
            if (socket == null)
            {
                return socket;
            }

            SocketState state = new SocketState(socket, -1);
            state.callMe = callMe;
            state.theSocket.BeginConnect(ipAddress, Networking.DEFAULT_PORT, ConnectedToServer, state);
            return socket;
        }

        /// <summary>
        /// This function is "called" by the operating system when the remote site acknowledges connect request
        /// </summary>
        /// <param name="ar"></param>
        private static void ConnectedToServer(IAsyncResult ar)
        {
            SocketState state = (SocketState)ar.AsyncState;

            try
            {
                // Complete the connection.
                state.theSocket.EndConnect(ar);
                System.Diagnostics.Debug.WriteLine("connected!");
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine("Unable to connect to server. Error occured: " + e);
                return;
            }

            // Start an event loop to receive data from the server.
            state.callMe(state);
        }

        /// <summary>
        /// handshake when it gets created
        /// </summary>
        /// <param name="ar"></param>
        private static void ReceiveCallback(IAsyncResult ar)
        {
            SocketState state = (SocketState)ar.AsyncState;
            try
            {

                int bytesRead = state.theSocket.EndReceive(ar);

                // If the socket is still open
                if (bytesRead > 0)
                {
                    string theMessage = Encoding.UTF8.GetString(state.messageBuffer, 0, bytesRead);
                    // Append the received data to the growable buffer.
                    // It may be an incomplete message, so we need to start building it up piece by piece
                    state.sb.Append(theMessage);
                }

                // Continue the "event loop" that was started on line 96.
                // Start listening for more parts of a message, or more new messages
                state.callMe(state);
            }
            catch (System.ObjectDisposedException ode)
            {
                foreach (var s in connectedSockets)
                {
                    disconnect((Socket)s.Value);
                }
            }


        }

        private static void disconnect(Socket socket)
        {
            socket.Disconnect(true);
        }

        /// <summary>
        /// A callback invoked when a send operation completes
        /// </summary>
        /// <param name="ar"></param>
        private static void SendCallback(IAsyncResult ar)
        {
            try
            {
                SocketState ss = (SocketState)ar.AsyncState;
                // Nothing much to do here, just conclude the send operation so the socket is happy.
                ss.theSocket.EndSend(ar);
            }
            catch
            {

            }

        }

        /// <summary>
        /// Sends a string to a connected socket
        /// </summary>
        /// <param name="ss">The socketstate containing the socket</param>
        /// <param name="message">The message to send</param>
        public static void Send(SocketState ss, string message)
        {

            message += (char)3;
            // Append a newline, since that is our protocol's terminating character for a message.
            try
            {
                //TODO we shouldn't have a new line
                byte[] messageBytes = Encoding.UTF8.GetBytes(message);
                ss.theSocket.BeginSend(messageBytes, 0, messageBytes.Length, SocketFlags.None, SendCallback, ss);
            }
            catch
            {
                Networking.connectedSockets.Remove(ss.ID);
            }

        }

        /// <summary>
        /// Requests data from a socket state and adds it to the sockets stringbuilder when 
        /// </summary>
        /// <param name="state"></param>
        public static void GetData(SocketState state)
        {
            state.theSocket.BeginReceive(state.messageBuffer, 0, state.messageBuffer.Length, SocketFlags.None, ReceiveCallback, state);
        }

        /*public static void ServerAwaitingClientLoop(NetworkAction action) {
            TcpListener listener = new TcpListener(DEFAULT_PORT);
            listener.Start();
            ConnectionState cs = new ConnectionState(listener, action);
            listener.BeginAcceptSocket(AcceptNewClient, cs);
        }

        private static void AcceptNewClient(IAsyncResult ar) {

            ConnectionState cs = (ConnectionState)ar.AsyncState;
            Socket connectedSocket = cs.listener.EndAcceptSocket(ar);
            SocketState ss = new SocketState(connectedSocket, connectedSockets.Count + 1);
            connectedSockets.Add(ss.ID, ss.theSocket);

            ss.callMe = cs.callMe;
            ss.callMe(ss);
            cs.listener.BeginAcceptSocket(AcceptNewClient, cs);
        }*/
    }
}