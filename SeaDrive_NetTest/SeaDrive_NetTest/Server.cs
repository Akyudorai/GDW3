using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

using NetworkUtilities;

namespace SeaDrive_NetTest
{ 
    class Server
    {
        // SERVER SETTINGS
        private const int BUFFER_SIZE = 2048;
        private const int PORT = 8888;


        private static byte[] buffer = new byte[BUFFER_SIZE];
        private static Dictionary<Socket, Guid> clients = new Dictionary<Socket, Guid>();
        //private static List<Socket> clientSockets = new List<Socket>();
        private static Socket serverSoc = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

        
        

        static void Main(string[] args)
        {
            Console.Title = "Server";
            InitializeServer();
            Console.ReadLine();
            CloseAllSockets();
        }

        private static void InitializeServer()
        {
            Console.WriteLine("Setting up the server...");

            NetMessage test = new NetMessage("This is a test message", 0);
            Console.WriteLine(test.Message);

            serverSoc.Bind(new IPEndPoint(IPAddress.Parse("127.0.0.1"), PORT));
            serverSoc.Listen(0);  // 5 = backlog:  how many pending connections can exist
            serverSoc.BeginAccept(new AsyncCallback(AcceptCallback), null);
            Console.WriteLine("Server has successfully been started up.");
        }

        private static void CloseAllSockets()
        {
            foreach (Socket s in clients.Keys)
            {
                s.Shutdown(SocketShutdown.Both);
                s.Close();
            }

            serverSoc.Close();
        }

        private static void AcceptCallback(IAsyncResult ar)
        {
            Socket socket;

            try
            {
                socket = serverSoc.EndAccept(ar);
            } catch (ObjectDisposedException)
            {
                return;
            }
            
            //clientSockets.Add(socket);
            clients.Add(socket, Guid.NewGuid());
            Console.WriteLine("Client " + clients[socket].ToString() + " connected.");

            // Tell the client what its guid is for self-identification purposes
            NetMessage outgoingGUID = new NetMessage(clients[socket].ToString(), 1);
            byte[] serializedGUID = Serializer.Serialize(outgoingGUID);
            socket.BeginSend(serializedGUID, 0, serializedGUID.Length, SocketFlags.None, SendCallback, socket);

            //byte[] clientID = Encoding.ASCII.GetBytes(clients[socket].ToString());
            //socket.BeginSend(clientID, 0, clientID.Length, SocketFlags.None, SendCallback, socket);

            // Send out an announcement to the other clients that someone has joined
            NetMessage newClientAnnoucement = new NetMessage(clients[socket].ToString() + " has joined the server!", 0);
            byte[] serializedAnnouncement = Serializer.Serialize(outgoingGUID);
            SyncSend(serializedAnnouncement, socket);
            //byte[] newClientAnnoucement = Encoding.ASCII.GetBytes(clients[socket].ToString() + " has joined the server!");
            //SyncSend(newClientAnnoucement, socket);

            socket.BeginReceive(buffer, 0, BUFFER_SIZE, SocketFlags.None, ReceiveCallback, socket);
            serverSoc.BeginAccept(AcceptCallback, null);
        }

        private static void ReceiveCallback(IAsyncResult ar)
        {
            Socket fromSoc = (Socket)ar.AsyncState;
            int recv = fromSoc.EndReceive(ar);
            byte[] dataBuf = new byte[recv];
            Array.Copy(buffer, dataBuf, recv);


            NetMessage result = Serializer.Deserialize<NetMessage>(dataBuf);
            //string text = Encoding.ASCII.GetString(dataBuf);
            if (result.Type == 0)
            {
                Console.WriteLine(clients[fromSoc].ToString() + ": " + result.Message);
            }
            

            if (result.Message.ToLower() == "get time")
            {
                NetMessage message  = new NetMessage(DateTime.Now.ToLongTimeString(), 0);
                byte[] data = Serializer.Serialize(message);
                //byte[] data = Encoding.ASCII.GetBytes(DateTime.Now.ToLongTimeString());
                fromSoc.BeginSend(data, 0, data.Length, SocketFlags.None, new AsyncCallback(SendCallback), fromSoc);
            } 
            
            else if (result.Message.ToLower() == "exit")
            {
                // Always shutdown before closing
                fromSoc.Shutdown(SocketShutdown.Both);
                fromSoc.Close();
                Console.WriteLine("Client " + clients[fromSoc].ToString() + " Disconnected");
                clients.Remove(fromSoc);               
                return;
            }
            
            else
            {
                NetMessage remote_msg = new NetMessage(clients[fromSoc].ToString() + ": " + result.Message, 0);
                byte[] remote_data = Serializer.Serialize(remote_msg);
                //byte[] remote_data = Encoding.ASCII.GetBytes("Remote Client: " + result.message);                
                SyncSend(remote_data, fromSoc);

                NetMessage local_msg = new NetMessage("You: " + result.Message, 0);
                byte[] local_data = Serializer.Serialize(local_msg);
                //byte[] local_data = Encoding.ASCII.GetBytes("Message Sent: " + result.message);
                fromSoc.BeginSend(local_data, 0, local_data.Length, SocketFlags.None, new AsyncCallback(SendCallback), fromSoc);


            }


            fromSoc.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, new AsyncCallback(ReceiveCallback), fromSoc);
        }

        public static void SyncSend(byte[] data, Socket from)
        {            
            foreach (Socket clientSoc in clients.Keys)
            {
                if (clientSoc != from)
                {
                    clientSoc.BeginSend(data, 0, data.Length, SocketFlags.None, SendCallback, clientSoc);
                }
            }
        }

        private static void SendCallback(IAsyncResult ar)
        {
            Socket socket = (Socket)ar.AsyncState;
            socket.EndSend(ar);
        }

        //// UTILITY FUNCTIONS, MOVE ELSEWHERE
        //public static byte[] Serialize(object source)
        //{
        //    var formatter = new BinaryFormatter();
        //    var stream = new MemoryStream();
        //    formatter.Serialize(stream, source);
        //    return stream.ToArray();

        //    //var formatter = new BinaryFormatter();
        //    //using (var stream = new MemoryStream())
        //    //{
        //    //    formatter.Serialize(stream, source);
        //    //    return stream.ToArray();
        //    //}
        //}

        //public static T Deserialize<T>(byte[] source)
        //{
        //    var formatter = new BinaryFormatter();
        //    var stream = new MemoryStream(source);            
        //    T deserializedObj = (T)formatter.Deserialize(stream);
        //    return deserializedObj;
        //}

    }
}
