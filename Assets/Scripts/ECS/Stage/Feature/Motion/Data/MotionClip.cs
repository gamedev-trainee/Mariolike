using UnityEngine;

namespace Mariolike
{
    [CreateAssetMenu(fileName = "NewMotionClip.asset", menuName = "Mariolike/Motion Clip")]
    public class MotionClip : ScriptableObject
    {
        public ActionClip action = null;
    }
}
