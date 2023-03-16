using UnityEngine;

namespace Mariolike
{
    public class GroundTestModule
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

        public HitTestFlags update(Vector3 pos, ref Vector3 offset)
        {
            if (offset.y < 0f)
            {
                RaycastHit hit;
                float checkHeight = m_height * 0.5f - offset.y;
                if (Physics.BoxCast(pos + new Vector3(0f, m_height, 0f), new Vector3(m_radius, m_height * 0.5f, m_radius), Vector3.down, out hit, Quaternion.identity, checkHeight, m_layerMask))
                {
                    if (hit.collider.gameObject.GetInstanceID() != m_instanceID)
                    {
                        if (hit.point.y <= pos.y + m_stepOffset)
                        {
                            float nextY = hit.point.y;
                            offset.y = nextY - pos.y;
                            return HitTestFlags.HitGround;
                        }
                        else
                        {
                            return HitTestFlags.OnGround;
                        }
                    }
                }
            }
            return HitTestFlags.None;
        }
    }
}
