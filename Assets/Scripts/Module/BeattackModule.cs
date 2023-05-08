using UnityEngine;

namespace Mariolike
{
    public class BeattackModule
    {
        public enum BeattackStates
        {
            None,
            Beattacking,
        }

        public static readonly string BeattackStateParameter = "beattack_state";
        public static readonly string BeattackStateName = "beattack";

        private Animator m_animator = null;

        private bool m_beattacking = false;

        public void setAnimator(Animator value)
        {
            m_animator = value;
        }

        public void beattack()
        {
            if (m_beattacking) return;
            m_beattacking = true;
            onBeattackStart();
        }

        public bool update()
        {
            if (!m_beattacking) return false;
            if (m_animator != null)
            {
                AnimatorStateInfo stateInfo = m_animator.GetCurrentAnimatorStateInfo(0);
                if (stateInfo.IsName(BeattackStateName))
                {
                    if (stateInfo.normalizedTime >= 1f)
                    {
                        onBeattackEnd();
                        return true;
                    }
                }
                return false;
            }
            onBeattackEnd();
            return true;
        }

        protected void onBeattackStart()
        {
            if (m_animator == null) return;
            if (m_animator.GetInteger(BeattackStateParameter) > (int)BeattackStates.None) return;
            m_animator.SetInteger(BeattackStateParameter, (int)BeattackStates.Beattacking);
        }

        protected void onBeattackEnd()
        {
            m_beattacking = false;
            if (m_animator == null) return;
            if (m_animator.GetInteger(BeattackStateParameter) <= (int)BeattackStates.None) return;
            m_animator.SetInteger(BeattackStateParameter, (int)BeattackStates.None);
        }
    }
}
