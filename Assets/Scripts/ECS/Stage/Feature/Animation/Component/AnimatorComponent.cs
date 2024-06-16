using ECSlike;
using UnityEngine;

namespace Mariolike
{
    public partial class AnimatorComponent : IComponent
    {
        [ConfigField]
        public Animator animator = null;

        public void setParameter(string name, int value)
        {
            animator.SetInteger(name, value);
        }

        public int getParameterInt(string name)
        {
            return animator.GetInteger(name);
        }

        public bool isStateComplete(string name)
        {
            AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
            if (stateInfo.IsName(name))
            {
                if (stateInfo.normalizedTime >= 1f)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
