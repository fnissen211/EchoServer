using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace EchoServer
{
    public class Server
    {
        // Network port the server will listen on. (7 is commonly used for Echo Protocol)
        private const int PORT = 7;


        /// <summary>
        /// This method contains the logic to start the server and handle incoming clients.
        /// </summary>
        public void Start()
        {
            TcpListener listener = new TcpListener(IPAddress.Any, PORT);
            listener.Start();
            Console.WriteLine("Server started");

            while (true)
            {
                // This line blocks the execution until a client connects to the server.
                // When a client connects, a new TcpClient object is created to
                // handle the communication with that client.
                TcpClient client = listener.AcceptTcpClient();

                Console.WriteLine("Client incoming");
                Console.WriteLine($"remote (ip,port) = ({client.Client.RemoteEndPoint})");

                // This line creates a new task that runs asynchronously.
                // Task.Run allows the server to handle multiple clients concurrently.
                Task.Run(() =>
                {
                    TcpClient tmpClient = client;
                    DoOneClient(client);
                });

            }
        }

        /// <summary>
        /// This method is responsible for handling communication with a single client.
        /// </summary>
        /// <param name="sock">TcpClient object sock</param>
        private void DoOneClient(TcpClient sock)
        {
            using (StreamReader sr = new StreamReader(sock.GetStream()))
            using (StreamWriter sw = new StreamWriter(sock.GetStream()))
            {
                sw.AutoFlush = true;
                Console.WriteLine("Handle one client");

                // simple echo
                String? s = sr.ReadLine();
                sw.WriteLine(s);
            }

        }
    }
}
