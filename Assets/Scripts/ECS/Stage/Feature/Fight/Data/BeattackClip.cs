using UnityEngine;

namespace Mariolike
{
    [CreateAssetMenu(fileName = "NewBeattackClip.asset", menuName = "Mariolike/Beattack Clip")]
    public class BeattackClip : ScriptableObject
    {
        public string desc = string.Empty;
        //
        public ConditionInfo condition = new ConditionInfo();
        //
        public MotionClip motion = null;
        public MotionClip counterMotion = null;
    }
}
