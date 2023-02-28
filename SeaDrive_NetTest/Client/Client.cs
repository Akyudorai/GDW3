using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;

using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using System.IO;

using NetworkUtilities;

namespace SeaDrive_NetTest
{
    class Client
    {
        private static Socket clientSoc = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        private static string guid = "";

        static void Main(string[] args)
        {
            Console.Title = "Client";
            ConnectToServer();

            Thread receiver = new Thread(() =>
            {
                while (true)
                {
                    ReceiveResponse();
                }
            });

            receiver.Start();

            RequestLoop();

           // SendLoop();
            Console.ReadLine();
        }

        //private static void SendLoop()
        //{
        //    while (true)
        //    {
        //        Console.WriteLine("Enter a request to server: ");
        //        string req = Console.ReadLine();
        //        byte[] buffer = Encoding.ASCII.GetBytes(req);
        //        clientSoc.Send(buffer);
                
        //        byte[] recvBuf = new byte[1024];
        //        int recv = clientSoc.Receive(recvBuf);
        //        byte[] data = new byte[recv];
        //        Array.Copy(recvBuf, data, recv);
        //        Console.WriteLine("Received: " + Encoding.ASCII.GetString(data));
        //    }
        //}

        private static void ConnectToServer()
        {
            int attempts = 0;

            while (!clientSoc.Connected)
            {
                try
                {
                    attempts++;
                    Console.WriteLine("Connection attempts: " + attempts.ToString());
                    clientSoc.Connect(IPAddress.Loopback, 8888);
                }

                catch (SocketException)
                {
                    Console.Clear();
                    
                }
            }
            
            Console.Clear();
            Console.WriteLine("Connected To Server!");
        }

        private static void RequestLoop()
        {
            Console.WriteLine(@"<Type ""exit"" to properly disconnect client>");
            
            while (true)
            {
                SendRequest();
                //ReceiveResponse();
            }
        }

        /// <summary>
        ///  Close the socket and exit program
        /// </summary>
        private static void Exit()
        {
            //SendString("exit");  // Tell the server we're exiting
            SendMessage(new NetMessage("exit", 0));
            clientSoc.Shutdown(SocketShutdown.Both);
            clientSoc.Close();
            Environment.Exit(0);
        }

        private static void SendRequest()
        {
            NetworkUtilities.NetMessage m = new NetMessage(Console.ReadLine(), 0);
            //string req = Console.ReadLine();
            //SendString(req);
            SendMessage(m);

            if (m.Message.ToLower() == "exit")
            {
                Exit();
            }
        }

        //private static void SendString(string text)
        //{
        //    byte[] buf = Encoding.ASCII.GetBytes(text);
        //    clientSoc.Send(buf, 0, buf.Length, SocketFlags.None);
        //}

        private static void SendMessage(NetMessage msg)
        {            
            byte[] data = Serialize(msg);
            clientSoc.Send(data, 0, data.Length, SocketFlags.None);
        }

        private static void ReceiveResponse()
        {
            var buf = new byte[2048];
            int recv = clientSoc.Receive(buf, SocketFlags.None);
            if (recv == 0) return;

            //var data = new byte[recv];
            //Array.Copy(buf, data, recv);
            //string result = Encoding.ASCII.GetString(data);

            var data = new byte[recv];
            Array.Copy(buf, data, recv);
            NetMessage result = Deserialize<NetMessage>(data);

            if (result.Type == 0)
            {
                Console.WriteLine(result.Message);
            } 

            else
            {
                // DO SOMETHING BASED ON MESSAGE TYPE
                if (result.Type == 1)
                {
                    // IT'S A GUID.  SAVE A RECORD
                    SetGUID(result.Message);
                }
            }              
        }

        private static void SetGUID(string id)
        {
            guid = id;
            Console.WriteLine("You have joined the server as user " + guid);
        }

        // UTILITY FUNCTIONS, MOVE ELSEWHERE
        public static byte[] Serialize(object source)
        {
            var formatter = new BinaryFormatter();
            var stream = new MemoryStream();
            formatter.Serialize(stream, source);
            return stream.ToArray();

            //var formatter = new BinaryFormatter();
            //using (var stream = new MemoryStream())
            //{
            //    formatter.Serialize(stream, source);
            //    return stream.ToArray();
            //}
        }

        public static T Deserialize<T>(byte[] source)
        {
            var formatter = new BinaryFormatter();
            var stream = new MemoryStream(source);
            T deserializedObj = (T)formatter.Deserialize(stream);
            return deserializedObj;
        }
    }
}
