using UnityEngine;

namespace Mariolike
{
    public class GravityModule
    {
        private float m_gravity = 0f;

        public void setGravity(float value)
        {
            m_gravity = value;
        }

        public void update(float deltaTime, ref Vector3 offset)
        {
            offset.y -= m_gravity * deltaTime;
        }
    }
}
