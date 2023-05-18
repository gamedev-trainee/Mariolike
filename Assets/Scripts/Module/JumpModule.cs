using UnityEngine;

namespace Mariolike
{
    public class JumpModule
    {
        public enum JumpStates
        {
            None,
            Jumping,
        }

        public static readonly string JumpStateParameter = "jump_state";

        private Animator m_animator = null;
        private float m_gravity = 0f;
        private float m_jumpSpeed = 0f;

        private int m_currentJumpDir = 0;
        private float m_currentJumpSpeed = 0f;

        private bool m_jumpFalling = false;

        public void setAnimator(Animator value)
        {
            m_animator = value;
        }

        public void setGravity(float value)
        {
            m_gravity = value;
        }

        public void setJumpSpeed(float value)
        {
            m_jumpSpeed = value;
        }

        public void jump()
        {
            if (m_currentJumpDir == 1) return;
            m_currentJumpDir = 1;
            m_currentJumpSpeed = m_jumpSpeed;
            onJumpStart();
        }

        public void stop()
        {
            if (m_currentJumpDir == 0) return;
            m_currentJumpDir = 0;
            m_jumpFalling = false;
            onJumpEnd();
        }

        public void reset()
        {
            if (m_currentJumpDir == 0) return;
            m_currentJumpSpeed *= 0.5f;
        }

        public bool isJumping()
        {
            return m_currentJumpDir != 0;
        }

        public bool isJumpFalling()
        {
            return m_jumpFalling;
        }

        public void update(float deltaTime, ref Vector3 offset)
        {
            if (m_currentJumpDir != 0)
            {
                if (m_currentJumpSpeed > 0)
                {
                    offset.y += m_currentJumpSpeed * m_currentJumpDir * deltaTime;
                    m_currentJumpSpeed -= m_gravity * deltaTime;
                    if (m_currentJumpSpeed <= 0) m_currentJumpSpeed = 0;
                }

                m_jumpFalling = offset.y < 0f || m_currentJumpSpeed <= 0f;
            }
        }

        protected void onJumpStart()
        {
            if (m_animator == null) return;
            if ((JumpStates)m_animator.GetInteger(JumpStateParameter) > JumpStates.None) return;
            m_animator.SetInteger(JumpStateParameter, (int)JumpStates.Jumping);
        }

        protected void onJumpEnd()
        {
            if (m_animator == null) return;
            if ((JumpStates)m_animator.GetInteger(JumpStateParameter) <= JumpStates.None) return;
            m_animator.SetInteger(JumpStateParameter, (int)JumpStates.None);
        }
    }
}
