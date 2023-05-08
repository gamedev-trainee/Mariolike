using UnityEngine;

namespace Mariolike
{
    public class DeathModule
    {
        public enum DeathStates
        {
            None,
            Dying,
        }

        public static readonly string DeathStateParameter = "death_state";
        public static readonly string DeathStateName = "death";

        private Animator m_animator = null;

        private bool m_dying = false;

        public void setAnimator(Animator value)
        {
            m_animator = value;
        }

        public void kill()
        {
            if (m_dying) return;
            m_dying = true;
            onDeathStart();
        }

        public bool update()
        {
            if (!m_dying) return false;
            if (m_animator != null)
            {
                AnimatorStateInfo stateInfo = m_animator.GetCurrentAnimatorStateInfo(0);
                if (stateInfo.IsName(DeathStateName))
                {
                    if (stateInfo.normalizedTime >= 1f)
                    {
                        return true;
                    }
                }
                return false;
            }
            return true;
        }

        protected void onDeathStart()
        {
            if (m_animator == null) return;
            if (m_animator.GetInteger(DeathStateParameter) > (int)DeathStates.None) return;
            m_animator.SetInteger(DeathStateParameter, (int)DeathStates.Dying);
        }
    }
}
