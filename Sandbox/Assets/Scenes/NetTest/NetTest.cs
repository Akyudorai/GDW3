using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Text;

using System.Threading.Tasks;

using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using System.IO;

using NetworkUtilities;

public class NetTest : MonoBehaviour
{
    public static NetTest instance;
    public GameObject playerRef;
    public NetworkedPlayerController netPC;    

    public static string localGuid = "";

    // CLIENT
    private static Socket clientSoc = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
    private static List<Socket> otherClients = new List<Socket>();
    public static bool isConnected = false;
    public static bool isStarted = false;

    [Header("Prefabs")]
    public GameObject prefab_NetPlayerObj; 
   
    public delegate void StringDelegate(string s);
    public StringDelegate OnClientLoaded;

    private Queue<System.Action> actionQueue = new Queue<System.Action>();

    private void Awake() 
    {
        instance = this;
    }

    private void OnDestroy() 
    {
        //SendString("exit");  // Tell the server we're exiting
        SendMessage(new NetMessage("exit", 0)); // Tell the server we're exiting
        clientSoc.Shutdown(SocketShutdown.Both);
        clientSoc.Close();
    }

    private async void Start()
    {       
        OnClientLoaded += OnClientConnected;

        bool result = await Task.Factory.StartNew(ConnectToServer);         
        
        if (result) 
        {
            NetMessage guid = await Task.Factory.StartNew(GetClientConnected);
            localGuid = guid.Message;   
            OnClientLoaded?.Invoke(localGuid);         

            // Begin receiving callbacks for follow-up messages
            Thread receiver = new Thread(() => 
            {
                while (true) 
                {
                    ReceiveResponse();
                }
            });

            receiver.Start(); 
        }

        isStarted = true;
    }

    private void Update() 
    {

        if (isStarted) return;


        if (actionQueue.Count > 0) 
        {
            actionQueue.Dequeue()();
        }

        //SyncPosition();        
    }
    

    private static bool ConnectToServer() 
    {
        int attempts = 0;

        bool timeout = false;
        while (!clientSoc.Connected && !timeout) 
        {
            try 
            {
                attempts++;
                Debug.Log("Connection attempts: " + attempts.ToString());
                clientSoc.Connect(IPAddress.Parse("127.0.0.1"), 8888);
            }

            catch (SocketException) 
            {
                
            }


            if (attempts > 10) {
                timeout = true;
            }
        }        

        // Client has successfull connected to the server
        Debug.Log((timeout) ? "Connection Timed Out." : "Connected to the server!");
        return isConnected = ((timeout) ? false : true);            
    }

    // Synchronized Instantiation
    public void NetInstantiate(GameObject obj, Vector3 pos, Quaternion rot)
    {
        
    }

    private void OnClientConnected(string id) 
    {
        if (id == localGuid)
        {
            // Instantiate a new game object for the local player
            Vector3 randPos = new Vector3(Random.Range(-10, 10), 2, Random.Range(-10, 10));
            GameObject newPlayerObj = Instantiate<GameObject>(prefab_NetPlayerObj, randPos, Quaternion.identity);        
            playerRef = newPlayerObj;
            netPC = playerRef.GetComponent<NetworkedPlayerController>();                
            netPC.Initialize(id);  
        }

        // Instantiate an object for all clients connected to the server



              
    }

    private void SyncPosition() 
    {
        if (playerRef != null) 
        {
            //string pos = "x: " + playerRef.transform.position.x + ", y: " + playerRef.transform.position.y + ", z: " + playerRef.transform.position.z;
            //SendString(pos);
        }
    }

    // private static void SendString(string text) 
    // {
    //     byte[] buf = Encoding.ASCII.GetBytes(text);
    //     clientSoc.Send(buf, 0, buf.Length, SocketFlags.None);
    // }

    private static void SendMessage(NetMessage m) 
    {
        byte[] data = Serializer.Serialize(m);
        clientSoc.Send(data, 0, data.Length, SocketFlags.None);
    }
    
    private static NetMessage GetClientConnected() 
    {
        var buf = new byte[2048];
        int recv = clientSoc.Receive(buf, SocketFlags.None);
        if (recv == 0) return null;

        var data = new byte[recv];
        System.Array.Copy(buf, data, recv);
                
        //string result = Encoding.ASCII.GetString(data);        
        NetworkUtilities.NetMessage result = Serializer.Deserialize<NetworkUtilities.NetMessage>(data);
        return result;
    }

    private static NetMessage ReceiveResponse() 
    {
        var buf = new byte[2048];
        int recv = clientSoc.Receive(buf, SocketFlags.None);
        if (recv == 0) return null;

        var data = new byte[recv];
        System.Array.Copy(buf, data, recv);
                
        //string result = Encoding.ASCII.GetString(data);        
        NetworkUtilities.NetMessage result = Serializer.Deserialize<NetworkUtilities.NetMessage>(data);

        if (result.Type == 1) {        
              
            System.Action clientConnected = delegate { instance.OnClientLoaded?.Invoke(result.Message); };
            instance.actionQueue.Enqueue(clientConnected);
        }

        Debug.Log("["+result.Type+"] " + result.Message);
        return result;
    }  
}

