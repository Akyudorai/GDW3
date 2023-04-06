using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;
using System.Net;
using System.Net.Sockets;

public class Client : MonoBehaviour
{
    // ----------------------------------------------------------------------------
    // VARIABLES & COMPONENTS
    // ----------------------------------------------------------------------------

    public static Client instance;
    public static int dataBufferSize = 4096;
    public static bool isConnected = false;

    public IPAddress ip;
    public int port = 8888;
    public int localClientID = 0;
    public TCP tcp;
    public UDP udp;

    private delegate void PacketHandler(Packet _packet);
    private static Dictionary<int, PacketHandler> packetHandlers;

    // ----------------------------------------------------------------------------
    // MONOBEHAVIOR 
    // ----------------------------------------------------------------------------
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Debug.Log("Instance already exists, destroying object!");
            Destroy(this);
        }
    }

    private void OnApplicationQuit() 
    {
        Disconnect();
    }

    // ----------------------------------------------------------------------------
    // CLIENT FUNCTIONALITY
    // ----------------------------------------------------------------------------

    public static bool IsLocalPlayer(NetworkIdentity identity)
    {         
        return ((instance.localClientID == identity.localClientID) ? true : false);
    }

    public void ConnectToServer()
    {
        InitializeClientData();
        
        //isConnected = true;
        tcp = new TCP(); 
        tcp.Connect();
    }

    private void InitializeClientData()
    {
        packetHandlers = new Dictionary<int, PacketHandler>()
        {
            { (int)ServerPackets.welcome, ClientHandle.Welcome },
            { (int)ServerPackets.spawnPlayer, ClientHandle.SpawnPlayer },
            { (int)ServerPackets.playerPosition, ClientHandle.PlayerPosition },
            { (int)ServerPackets.playerRotation, ClientHandle.PlayerRotation },
            { (int)ServerPackets.chatMessage, ClientHandle.ChatMessage },
            { (int)ServerPackets.playerDisconnection, ClientHandle.PlayerDisconnection },
            { (int)ServerPackets.newHighScore, ClientHandle.NewHighScore },
            { (int)ServerPackets.joinMultiplayer, ClientHandle.JoinMultiplayer },
            { (int)ServerPackets.receiveAnimationState, ClientHandle.ReceiveAnimationState }
        };
        Debug.Log("Initialized packets.");
    }

    public void Disconnect()
    {
        if (isConnected) 
        {
            isConnected = false;
            tcp.socket.Close();
            udp.socket.Close();

            Debug.Log("Disconnected from server.");            
        }        
    }

    // ----------------------------------------------------------------------------
    // TCP 
    // ----------------------------------------------------------------------------

    public class TCP
    {
        public TcpClient socket;

        private NetworkStream stream;
        private Packet receivedData;
        private byte[] receiveBuffer;

        public void Connect()
        {
            socket = new TcpClient
            {
                ReceiveBufferSize = dataBufferSize,
                SendBufferSize = dataBufferSize
            };

            receiveBuffer = new byte[dataBufferSize];
            
            //instance.ip = IPAddress.Parse("127.0.0.1");
            instance.ip = IPAddress.Parse("3.216.13.20");        
            socket.BeginConnect(instance.ip, instance.port, ConnectCallback, socket);

            instance.udp = new UDP();
        }

        private void ConnectCallback(IAsyncResult _result)
        {
            socket.EndConnect(_result);
            Debug.Log(_result);
            if (!socket.Connected)
            {                            
                return;
            }            
            
            Debug.Log("Connected to server!");
            isConnected = true;
            stream = socket.GetStream();
            receivedData = new Packet();
            stream.BeginRead(receiveBuffer, 0, dataBufferSize, ReceiveCallback, null);
        }

        public void SendData(Packet _packet)
        {
            try
            {
                if (socket != null)
                {
                    stream.BeginWrite(_packet.ToArray(), 0, _packet.Length(), null, null);
                }
            }
            catch (Exception _ex)
            {
                Debug.Log($"Error sending data to server via TCP: {_ex}");
            }
        }

        private void ReceiveCallback(IAsyncResult _result)
        {
            try
            {
                int _byteLength = stream.EndRead(_result);
                if (_byteLength <= 0)
                {
                    instance.Disconnect();
                    return;
                }

                byte[] _data = new byte[_byteLength];
                Array.Copy(receiveBuffer, _data, _byteLength);

                receivedData.Reset(HandleData(_data));
                stream.BeginRead(receiveBuffer, 0, dataBufferSize, ReceiveCallback, null);
            }
            catch
            {
                Disconnect();
            }
        }

        private bool HandleData(byte[] _data)
        {
            int _packetLength = 0;

            receivedData.SetBytes(_data);

            if (receivedData.UnreadLength() >= 4)
            {
                _packetLength = receivedData.ReadInt();
                if (_packetLength <= 0)
                {
                    return true;
                }
            }

            while (_packetLength > 0 && _packetLength <= receivedData.UnreadLength())
            {
                byte[] _packetBytes = receivedData.ReadBytes(_packetLength);
                ThreadManager.ExecuteOnMainThread(() =>
                {
                    using (Packet _packet = new Packet(_packetBytes))
                    {
                        int _packetId = _packet.ReadInt();
                        packetHandlers[_packetId](_packet);
                    }
                });

                _packetLength = 0;
                if (receivedData.UnreadLength() >= 4)
                {
                    _packetLength = receivedData.ReadInt();
                    if (_packetLength <= 0)
                    {
                        return true;
                    }
                }
            }

            if (_packetLength <= 1)
            {
                return true;
            }

            return false;
        }

        public void Disconnect() 
        {
            instance.Disconnect();

            stream = null;
            receivedData = null;
            receiveBuffer = null;
            socket = null;
        }
    }

    // ----------------------------------------------------------------------------
    // UDP
    // ----------------------------------------------------------------------------

    public class UDP
    {
        public UdpClient socket;
        public IPEndPoint endPoint;

        public UDP()
        {
            endPoint = new IPEndPoint(instance.ip, instance.port);
        }

        public void Connect(int _localPort)
        {
            socket = new UdpClient(_localPort);

            socket.Connect(endPoint);
            socket.BeginReceive(ReceiveCallback, null);

            using (Packet _packet = new Packet())
            {
                SendData(_packet);
            }
        }

        public void SendData(Packet _packet)
        {
            try
            {
                _packet.InsertInt(instance.localClientID);
                if (socket != null)
                {
                    Debug.Log("Sending Data UDP");
                    socket.BeginSend(_packet.ToArray(), _packet.Length(), null, null);                    
                } else
                {
                    Debug.Log("Socket null");
                }
            }
            catch (Exception _ex)
            {
                Debug.Log($"Error sending data to server via UDP: {_ex}");
            }
        }

        private void ReceiveCallback(IAsyncResult _result)
        {
            try
            {
                byte[] _data = socket.EndReceive(_result, ref endPoint);
                socket.BeginReceive(ReceiveCallback, null);

                if (_data.Length < 4)
                {
                    instance.Disconnect();
                    return;
                }

                HandleData(_data);
            }
            catch
            {
                Disconnect();
            }
        }

        private void HandleData(byte[] _data)
        {
            using (Packet _packet = new Packet(_data))
            {
                int _packetLength = _packet.ReadInt();
                _data = _packet.ReadBytes(_packetLength);
            }

            ThreadManager.ExecuteOnMainThread(() =>
            {
                using (Packet _packet = new Packet(_data))
                {
                    int _packetId = _packet.ReadInt();
                    packetHandlers[_packetId](_packet);
                }
            });
        }

        private void Disconnect() 
        {
            instance.Disconnect();

            endPoint = null;
            socket = null;
        }
    }

}