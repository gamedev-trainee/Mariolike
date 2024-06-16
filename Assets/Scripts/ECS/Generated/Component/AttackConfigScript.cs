/////////////////////////////////////////////////
//
// Generated By ECSlikeGenerator
//
/////////////////////////////////////////////////

using UnityEngine;

namespace Mariolike
{
    [DisallowMultipleComponent]
    public class AttackConfigScript : MonoBehaviour, ECSlike.IComponentConfig
    {
        public System.Collections.Generic.List<AttackClip> attackClips = new System.Collections.Generic.List<AttackClip>();

        private void OnDrawGizmos()
        {
            AttackComponent.DrawAttackClips(transform, attackClips);
        }
    }
}