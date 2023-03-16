using UnityEngine;

namespace Mariolike
{
    public class MoveModule
    {
        private float m_moveSpeed = 0f;

        private int m_lastMoveDir = 0;
        private int m_currentMoveDir = 0;
        private int m_moveForward = 0;
        private System.Action<int> m_moveForwardChangeCallback = null;

        public void setMoveSpeed(float value)
        {
            m_moveSpeed = value;
        }

        public void setMoveForwardChangeCallback(System.Action<int> value)
        {
            m_moveForwardChangeCallback = value;
        }

        public int getMoveForward()
        {
            return m_moveForward;
        }

        public void moveRight()
        {
            m_currentMoveDir = 1;
        }

        public void moveLeft()
        {
            m_currentMoveDir = -1;
        }

        public void moveRandom()
        {
            m_currentMoveDir = Random.Range(0, 100) % 2 == 0 ? -1 : 1;
        }

        public void reverse()
        {
            m_currentMoveDir = -m_currentMoveDir;
        }

        public void stop()
        {
            m_currentMoveDir = 0;
        }

        public void update(float deltaTime, ref Vector3 offset)
        {
            if (m_currentMoveDir != 0)
            {
                if (m_lastMoveDir != m_currentMoveDir)
                {
                    m_lastMoveDir = m_currentMoveDir;
                    m_moveForward = m_currentMoveDir;
                    m_moveForwardChangeCallback?.Invoke(m_moveForward);
                }
                offset.x += m_moveSpeed * m_currentMoveDir * deltaTime;
            }
        }
    }
}
