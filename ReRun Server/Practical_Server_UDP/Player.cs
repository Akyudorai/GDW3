using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Numerics;

namespace Practical_Server_UDP
{
    class Player
    {
        public int id;
        public string username = "[NULL]";
        public int character = 1;

        public Vector3 position = Vector3.Zero;
        public Quaternion rotation = Quaternion.Identity;

        public Player(int _id, string _username, Vector3 _spawnPosition)
        {
            id = _id;
            username = _username;
            position = _spawnPosition;
            rotation = Quaternion.Identity;
        }

        public void Update()
        {
            // Do nothing?  Maybe useful later
            ServerSend.PlayerPosition(this);
        }

        public void UpdateTransform(Vector3 _position, Quaternion _rotation)
        {
            position = _position;
            rotation = _rotation;

            ServerSend.PlayerPosition(this);
            ServerSend.PlayerRotation(this);
        }
    }
}
