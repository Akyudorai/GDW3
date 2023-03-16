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
        public float[] Scores = new float[10];       

        public static Leaderboard Default()
        {
            Leaderboard result = new Leaderboard();

            result.Scores = new float[10];
            for (int i = 0; i < result.Scores.Length; i++)
            {
                result.Scores[i] = 0.0f;
            }

            return result;
        }
    }
}
