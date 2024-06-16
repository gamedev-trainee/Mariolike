using UnityEngine;

namespace Mariolike
{
    public abstract class TimeActionClip : ActionClip
    {
        public float duration = 0f;
        public AnimationCurve curve = AnimationCurve.Linear(0f, 0f, 1f, 1f);
    }
}
