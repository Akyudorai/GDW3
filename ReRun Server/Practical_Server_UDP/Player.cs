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
        public string username;
        public int character;

        public Vector3 position;
        public Quaternion rotation;

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
