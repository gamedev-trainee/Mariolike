using System.Collections.Generic;
using UnityEngine;

namespace Mariolike
{
    [CreateAssetMenu(fileName = "NewAttackClip.asset", menuName = "Mariolike/Attack Clip")]
    public class AttackClip : ScriptableObject
    {
        public string desc = string.Empty;
        public Color color = Color.white;
        //
        public Vector3 rangeOffset = Vector3.zero;
        public Vector3 rangeSize = Vector3.zero;
        //
        public LayerMask layerMask = 0;
        //
        public FightConnections connection = FightConnections.Opposite;
        //
        public MotionClip targetMotion = null;
    }
}
