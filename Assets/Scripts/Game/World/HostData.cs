using System.Collections.Generic;
using UnityEngine;

namespace Mariolike
{
    public class HostData
    {
        public int life = 0;
        public int score = 0;
        public float time = 0;
        public Vector3 scale = Vector3.one;
        public Dictionary<AttrTypes, int> attrs = new Dictionary<AttrTypes, int>();
    }
}
