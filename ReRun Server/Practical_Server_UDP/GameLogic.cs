using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json;

namespace Practical_Server_UDP
{
    class GameLogic
    {
        public static Dictionary<int, Leaderboard> Leaderboards = new Dictionary<int, Leaderboard>();

        public static void Start()
        {
            LoadLeaderboards();            
        }

        public static void Update()
        {
            foreach (Client _client in Server.clients.Values)
            {
                if (_client.player != null)
                {
                    _client.player.Update();
                }
            }

            ThreadManager.UpdateMain();
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
            Console.WriteLine($"DEBUG: Checking score of {_time} for race with ID of {_raceID}");

            int result = -1;
            bool canSetPosition = true;

            string prevName = _name;
            float prevTime = _time;
            for (int i = 0 ; i < Leaderboards[_raceID].Entries.Length; i++)
            {                
                Console.WriteLine($"Comparing time of {_time} to {Leaderboards[_raceID].Entries[i].Time}");
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
