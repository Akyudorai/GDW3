using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Numerics;

namespace Practical_Server_UDP
{
    class ServerHandle
    {
        public static void WelcomeReceived(int _fromClient, Packet _packet)
        {
            int _clientIdCheck = _packet.ReadInt();
            string _username = _packet.ReadString();

            Console.WriteLine($"{Server.clients[_fromClient].tcp.socket.Client.RemoteEndPoint} connected successfully and is now player {_fromClient}.");
            if (_fromClient != _clientIdCheck)
            {
                Console.WriteLine($"Player \"{_username}\" (ID: {_fromClient}) has assumed the wrong client ID ({_clientIdCheck})!");
            }              
        }

        public static void PlayerMovement(int _fromClient, Packet _packet)
        {                       
            Vector3 _position = _packet.ReadVector3();
            Quaternion _rotation = _packet.ReadQuaternion();            

            if (Server.clients[_fromClient].player != null)
            {
                Server.clients[_fromClient].player.UpdateTransform(_position, _rotation);
            }            
        }

        public static void ChatMessage(int _fromClient, Packet _packet)
        {   
            int _id = _packet.ReadInt();
            string _msg = _packet.ReadString();
            
            
            if (_msg.Equals("quit", StringComparison.InvariantCultureIgnoreCase))
            {
                // Do nothing.  let the server handle the disconnection
            }

            else
            {
                ServerSend.ChatMessage(Server.clients[_id].player, _msg);
            }            
        }

        public static void SubmitScore(int _fromClient, Packet _packet)
        {
            string _username = _packet.ReadString();
            int _raceID = _packet.ReadInt();
            float _score = _packet.ReadFloat();
            
            int result = GameLogic.CheckScore(_raceID, _score, _username);

            if (result != -1)
            {
                Console.WriteLine($"New Highscore Achieved by {_username} in race {_raceID}, earning the position {result} with a time of {_score}");
                ServerSend.NewHighScore(_username, result, _raceID, _score);
            }
        }

        public static void JoinMultiplayer(int _fromClient, Packet _packet)
        {
            int _clientID = _packet.ReadInt();
            string _username = _packet.ReadString();
            int _character = _packet.ReadInt();

            //GameLogic.ActiveClients.Add(_clientID, Server.clients[_clientID]);
            GameLogic.SendIntoGame(Server.clients[_clientID], _username, _character);
        }

        public static void ReceiveAnimationState(int _fromClient, Packet _packet)
        {
            int _clientID = _packet.ReadInt();
            float _movement = _packet.ReadFloat();
            bool _isGrounded = _packet.ReadBool();
            bool _splineControlled = _packet.ReadBool();
            bool _isWallRunning = _packet.ReadBool();
            bool _isWallRunningRight = _packet.ReadBool();
            bool _isRailGrinding = _packet.ReadBool();
            bool _isZiplining = _packet.ReadBool();
            bool _isLedgeGrabbing = _packet.ReadBool();

            ServerSend.SendAnimationState(_clientID, _movement, _isGrounded, _splineControlled, _isWallRunning, _isWallRunningRight, _isRailGrinding, _isZiplining, _isLedgeGrabbing);
        }
    }
}
