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

        public static bool CheckScore(int _raceID, float _score, int _clientID)
        {
            bool result = false;

            // TODO: Loop through leaderboard and check if the score beats any in the array.  Begin from first element


            return result;
        }

        
    }
}
