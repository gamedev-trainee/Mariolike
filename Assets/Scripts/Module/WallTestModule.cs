using UnityEngine;

namespace Mariolike
{
    public class WallTestModule
    {
        private int m_instanceID = 0;
        private float m_radius = 0f;
        private float m_height = 0f;
        private float m_stepOffset = 0f;
        private int m_layerMask = 0;

        public void setInstanceID(int value)
        {
            m_instanceID = value;
        }

        public void setRadius(float value)
        {
            m_radius = value;
        }

        public void setHeight(float value)
        {
            m_height = value;
        }

        public void setStepOffset(float value)
        {
            m_stepOffset = value;
        }

        public void setLayerMask(int value)
        {
            m_layerMask = value;
        }

        public HitTestFlags update(Vector3 pos, int forward, ref Vector3 offset)
        {
            if (offset.sqrMagnitude > 0)
            {
                float checkDistance = offset.x * forward + m_radius;
                Vector3 pointBottom = pos + new Vector3(0f, m_radius, 0f) + new Vector3(-forward * m_radius, 0f, 0f);
                Vector3 pointTop = pos + new Vector3(0f, m_radius + m_height, 0f) + new Vector3(-forward * m_radius, 0f, 0f);
                RaycastHit[] hits = Physics.CapsuleCastAll(pointBottom, pointTop, m_radius, new Vector3(forward, 0f, 0f), checkDistance, m_layerMask);
                if (hits != null && hits.Length > 0)
                {
                    int count = hits.Length;
                    for (int i = 0; i < count; i++)
                    {
                        if (hits[i].collider.gameObject.GetInstanceID() == m_instanceID) continue;
                        if (hits[i].point.y > pos.y + offset.y + m_stepOffset)
                        {
                            float nextX = hits[i].point.x - m_radius * forward;
                            offset.x = nextX - pos.x;
                            return HitTestFlags.HitWall;
                        }
                    }
                }
            }
            return HitTestFlags.None;
        }
    }
}
