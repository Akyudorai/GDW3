﻿using System;
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



        public static void Welcome(int _toClient, string _msg)
        {
            using (Packet _packet = new Packet((int)ServerPackets.welcome))
            {
                _packet.Write(_msg);
                _packet.Write(_toClient);
                SendTCPData(_toClient, _packet);
            }
        }

        public static void Disconnection(Player _player)
        {
            using (Packet _packet = new Packet((int)ServerPackets.playerDisconnection))
            {
                _packet.Write(_player.id);
                SendTCPDataToAll(_player.id, _packet);
            }
        }

        public static void SpawnPlayer(int _toClient, Player _player)
        {
            using (Packet _packet = new Packet((int)ServerPackets.spawnPlayer))
            {
                _packet.Write(_player.id);
                _packet.Write(_player.username);
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

                SendUDPDataToAll(_packet);
            }
        }

        public static void PlayerRotation(Player _player)
        {
            using (Packet _packet = new Packet((int)ServerPackets.playerRotation))
            {
                _packet.Write(_player.id);
                _packet.Write(_player.rotation);

                SendUDPDataToAll(_player.id, _packet);
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


    }
}