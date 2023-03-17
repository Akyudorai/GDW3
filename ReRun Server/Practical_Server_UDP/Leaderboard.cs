using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Practical_Server_UDP
{ 
    [System.Serializable]
    public class Leaderboard
    {
        public class Entry
        {
            public string Name;
            public float Time;            
        }

        public Entry[] Entries = new Entry[10];       

        public static Leaderboard Default()
        {
            Leaderboard result = new Leaderboard();

            result.Entries = new Entry[10];
            for (int i = 0; i < result.Entries.Length; i++)
            {
                result.Entries[i].Name = "----";
                result.Entries[i].Time = 0.0f;
            }

            return result;
        }
          
    }
}
