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
            public string Name = "[NONE]";
            public float Time = 3600;            
        }

        public Entry[] Entries = new Entry[10];       

        public static Leaderboard Default()
        {
            Leaderboard result = new Leaderboard();

            result.Entries = new Entry[10];
            for (int i = 0; i < result.Entries.Length; i++)
            {
                result.Entries[i] = new Entry();
                result.Entries[i].Name = "[NONE]";
                result.Entries[i].Time = 3600.0f;
            }

            return result;
        }
          
    }
}
