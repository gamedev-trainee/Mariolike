using UnityEngine;

namespace Mariolike
{
    public class JumpModule
    {
        private float m_gravity = 0f;
        private float m_jumpSpeed = 0f;

        private int m_currentJumpDir = 0;
        private float m_currentJumpSpeed = 0f;

        private bool m_jumpFalling = false;

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
            m_currentJumpDir = 1;
            m_currentJumpSpeed = m_jumpSpeed;
        }

        public void stop()
        {
            m_currentJumpDir = 0;
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
                offset.y += m_currentJumpSpeed * m_currentJumpDir * deltaTime;
                m_currentJumpSpeed -= m_gravity * deltaTime;

                m_jumpFalling = offset.y < 0f;
            }
        }
    }
}
