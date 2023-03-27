using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json;
using System.Numerics;

namespace Practical_Server_UDP
{
    class GameLogic
    {
        public static Dictionary<int, Leaderboard> Leaderboards = new Dictionary<int, Leaderboard>();
        public static Dictionary<int, Client> ActiveClients = new Dictionary<int, Client>();

        public static void Start()
        {
            LoadLeaderboards();            
        }

        public static void Update()
        {
            foreach (Client _client in ActiveClients.Values)
            {
                if (_client.player != null)
                {
                    _client.player.Update();
                }
            }

            ThreadManager.UpdateMain();
        }

        public static void SendIntoGame(Client _client, string _playerName, int _character)
        {
            _client.player = new Player(_client.id, _playerName, new Vector3(0, 0, 0));
            _client.player.character = _character;
            ActiveClients.Add(_client.id, _client);

            foreach (Client _c in ActiveClients.Values)
            {
                // Spawn a player for each existing player on the server (not including self)
                if (_c.player != null)
                {
                    if (_c.id != _client.id)
                    {
                        ServerSend.SpawnPlayer(_client.id, _c.player, _c.player.character);
                    }
                }
            }

            foreach (Client _c in ActiveClients.Values)
            {
                if (_c.player != null)
                {
                    ServerSend.SpawnPlayer(_c.id, _client.player, _client.player.character);
                }
            }
        }

        public static void Disconnect(int _clientID)
        {
            ActiveClients.Remove(_clientID);
        }

        private static void SaveLeaderboards()
        {
            var StartupDirectory = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            string path = StartupDirectory + @"\Leaderboards.json";
            string json = JsonConvert.SerializeObject(Leaderboards, Formatting.Indented);
            System.IO.File.WriteAllText(path, json);

        }

        private static void LoadLeaderboards()
        {
            var StartupDirectory = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            string path = StartupDirectory + @"\Leaderboards.json";

            if (System.IO.File.Exists(path))
            {
                string json = System.IO.File.ReadAllText(path);
                Leaderboards = JsonConvert.DeserializeObject<Dictionary<int, Leaderboard>>(json);                
            }

            else
            {
                Console.WriteLine($"Failed to load file at path {path}");
            }            
        }

        public static int CheckScore(int _raceID, float _time, string _name)
        {           
            int result = -1;
            bool canSetPosition = true;

            string prevName = _name;
            float prevTime = _time;
            for (int i = 0 ; i < Leaderboards[_raceID].Entries.Length; i++)
            {                
                if (prevTime < Leaderboards[_raceID].Entries[i].Time)
                {
                    if (canSetPosition) {
                        result = i;
                        canSetPosition = false;                        
                    }

                    float temp = Leaderboards[_raceID].Entries[i].Time;
                    Leaderboards[_raceID].Entries[i].Name = prevName;
                    Leaderboards[_raceID].Entries[i].Time = prevTime;
                    prevTime = temp;
                }
            }

            if (result != -1) SaveLeaderboards();

            return result;
        }

        
    }
}
