using UnityEngine;

namespace Mariolike
{
    public class GravityModule
    {
        private float m_gravity = 0f;

        private float m_currentGravity = 0f;

        public void setGravity(float value)
        {
            m_gravity = value;
        }

        public void resetGravity()
        {
            m_currentGravity = m_gravity;
        }

        public void update(float deltaTime, ref Vector3 offset)
        {
            offset.y -= m_currentGravity * deltaTime;
            m_currentGravity += m_gravity * deltaTime;
        }
    }
}
