using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetworkUtilities
{
    [Serializable]
    public class NetMessage
    {
        // 0 = normal, 1 = clientConnect
        public int Type;
        public string Message;

        public NetMessage(string msg, int type = 0)
        {
            Message = msg;
            Type = type;
        }
    }
}
