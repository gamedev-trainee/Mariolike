/////////////////////////////////////////////////
//
// Generated By ECSlikeGenerator
//
/////////////////////////////////////////////////

using UnityEngine;

namespace Mariolike
{
    [DisallowMultipleComponent]
    public class DeathConfigScript : MonoBehaviour, ECSlike.IComponentConfig
    {
        public ConditionInfo condition = new Mariolike.ConditionInfo();
        public MotionClip motion = null;
    }
}