using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Practical_Server_UDP
{
    class ServerSend
    {
        private static void SendTCPData(int _toClient, Packet _packet)
        {
            _packet.WriteLength();
            Server.clients[_toClient].tcp.SendData(_packet);
        }

        private static void SendUDPData(int _toClient, Packet _packet)
        {
            _packet.WriteLength();
            Server.clients[_toClient].udp.SendData(_packet);
        }

        private static void SendTCPDataToAll(Packet _packet)
        {
            _packet.WriteLength();
            for (int i = 1; i <= Server.MaxPlayers; i++)
            {
                Server.clients[i].tcp.SendData(_packet);
            }
        }

        private static void SendUDPDataToAll(Packet _packet)
        {
            _packet.WriteLength();
            for (int i = 1; i <= Server.MaxPlayers; i++)
            {
                Server.clients[i].udp.SendData(_packet);
            }
        }

        private static void SendTCPDataToAll(int _clientException, Packet _packet)
        {
            _packet.WriteLength();
            for (int i = 1; i <= Server.MaxPlayers; i++)
            {
                if (i != _clientException)
                {
                    Server.clients[i].tcp.SendData(_packet);
                }                
            }
        }
        

        private static void SendUDPDataToAll(int _clientException, Packet _packet)
        {
            _packet.WriteLength();
            for (int i = 1; i <= Server.MaxPlayers; i++)
            {
                if (i != _clientException)
                {
                    Server.clients[i].udp.SendData(_packet);
                }
            }
        }

        private static void SendTCPDataToAllConnected(Packet _packet)
        {
            _packet.WriteLength();
            foreach (Client c in GameLogic.ActiveClients.Values)
            {
                c.tcp.SendData(_packet);
            }
            
            //for (int i = 1; i <= GameLogic.ActiveClients.Values.Count; i++)
            //{
            //    GameLogic.ActiveClients[i].tcp.SendData(_packet);
            //}
        }

        private static void SendUDPDataToAllConnected(Packet _packet)
        {
            _packet.WriteLength();

            foreach (Client c in GameLogic.ActiveClients.Values)
            {
                c.udp.SendData(_packet);
            }

            //for (int i = 1; i <= GameLogic.ActiveClients.Values.Count; i++)
            //{
            //    GameLogic.ActiveClients[i].udp.SendData(_packet);
            //}
        }

        private static void SendTCPDataToAllConnected(int _clientException, Packet _packet)
        {
            _packet.WriteLength();

            foreach (Client c in GameLogic.ActiveClients.Values)
            {
                if (c.id != _clientException)
                {
                    c.tcp.SendData(_packet);
                }
            }

            //for (int i = 1; i <= GameLogic.ActiveClients.Values.Count; i++)
            //{
            //    if (i != _clientException)
            //    {
            //        GameLogic.ActiveClients[i].tcp.SendData(_packet);
            //    }
            //}
        }

        private static void SendUDPDataToAllConnected(int _clientException, Packet _packet)
        {
            _packet.WriteLength();

            foreach (Client c in GameLogic.ActiveClients.Values)
            {
                if (c.id != _clientException)
                {
                    c.udp.SendData(_packet);
                }
            }

            //for (int i = 1; i <= GameLogic.ActiveClients.Values.Count; i++)
            //{
            //    if (i != _clientException)
            //    {
            //        GameLogic.ActiveClients[i].udp.SendData(_packet);
            //    }
            //}
        }



        public static void Welcome(int _toClient, string _msg)
        {
            using (Packet _packet = new Packet((int)ServerPackets.welcome))
            {
                _packet.Write(_msg);
                _packet.Write(_toClient);
                SendTCPData(_toClient, _packet);
            }
        }

        public static void Disconnection(int _playerID)
        {
            using (Packet _packet = new Packet((int)ServerPackets.playerDisconnection))
            {
                _packet.Write(_playerID);
                SendTCPDataToAll(_playerID, _packet);
            }
        }

        public static void SpawnPlayer(int _toClient, Player _player, int _character)
        {
            using (Packet _packet = new Packet((int)ServerPackets.spawnPlayer))
            {
                _packet.Write(_player.id);
                _packet.Write(_player.username);
                _packet.Write(_character);
                _packet.Write(_player.position);
                _packet.Write(_player.rotation);
                

                SendTCPData(_toClient, _packet);
            }
        }

        public static void PlayerPosition(Player _player)
        {
            using (Packet _packet = new Packet((int)ServerPackets.playerPosition))
            {
                _packet.Write(_player.id);
                _packet.Write(_player.position);
                
                /// UDP does not seem to be working on AWS
                //SendUDPDataToAllConnected(_packet);
                SendTCPDataToAllConnected(_packet);
            }
        }

        public static void PlayerRotation(Player _player)
        {
            using (Packet _packet = new Packet((int)ServerPackets.playerRotation))
            {
                _packet.Write(_player.id);
                _packet.Write(_player.rotation);

                /// UDP does not seem to be working on AWS
                //SendUDPDataToAllConnected(_player.id, _packet);
                SendTCPDataToAllConnected(_player.id, _packet);
            }
        }

        public static void ChatMessage(Player _player, string _msg)
        {
            using (Packet _packet = new Packet((int)ServerPackets.chatMessage))
            {
                _packet.Write(_player.id);
                _packet.Write(_msg);

                SendTCPDataToAll(_packet);
            }
        }

        public static void NewHighScore(string _playerName, int _position, int _raceID, float _time)
        {
            using (Packet _packet = new Packet((int)ServerPackets.newHighScore))
            {
                _packet.Write(_playerName);
                _packet.Write(_position);
                _packet.Write(_raceID);
                _packet.Write(_time);

                SendTCPDataToAll(_packet);
            }
        }

        public static void JoinMultiplayer(int _clientID)
        {
            using (Packet _packet = new Packet((int)ServerPackets.joinMultiplayer))
            {
                _packet.Write(_clientID);
                
                SendTCPDataToAll(_packet);
            }
        }

        public static void SendAnimationState(int _clientID, float _movement, bool _isGrounded, bool _splineControlled, bool _isWallRunning, bool _isWallRunRight, bool _isRailGrinding, bool _isZiplining, bool _isLedgeGrabbing)
        {
            using (Packet _packet = new Packet((int)ServerPackets.receiveAnimationState))
            {
                _packet.Write(_clientID);
                _packet.Write(_movement);
                _packet.Write(_isGrounded);
                _packet.Write(_splineControlled);
                _packet.Write(_isWallRunning);
                _packet.Write(_isWallRunRight);
                _packet.Write(_isRailGrinding);
                _packet.Write(_isZiplining);
                _packet.Write(_isLedgeGrabbing);

                /// UDP does not seem to be working on AWS
                //SendUDPDataToAllConnected(_clientID, _packet);
                SendTCPDataToAllConnected(_clientID, _packet);
            }
        }


    }
}
