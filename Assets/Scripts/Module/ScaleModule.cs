using UnityEngine;

namespace Mariolike
{
    public class ScaleModule
    {
        private float m_scaleSpeed = 0f;

        private bool m_scaling = false;
        private float m_scaleTo = 0f;

        public void setScaleSpeed(float value)
        {
            m_scaleSpeed = value;
        }

        public void scaleTo(float value)
        {
            if (m_scaling) return;
            m_scaling = true;
            m_scaleTo = value;
        }

        public bool update(float deltaTime, ref Vector3 scale)
        {
            if (m_scaling)
            {
                float currentScale = scale.x;
                if (m_scaleTo > currentScale)
                {
                    currentScale += m_scaleSpeed * deltaTime;
                    if (currentScale >= m_scaleTo)
                    {
                        currentScale = m_scaleTo;
                        m_scaling = false;
                    }
                    scale.Set(currentScale, currentScale, currentScale);
                    return true;
                }
                if (m_scaleTo < currentScale)
                {
                    currentScale -= m_scaleSpeed * deltaTime;
                    if (currentScale <= m_scaleTo)
                    {
                        currentScale = m_scaleTo;
                        m_scaling = false;
                    }
                    scale.Set(currentScale, currentScale, currentScale);
                    return true;
                }
                m_scaling = false;
            }
            return false;
        }
    }
}
