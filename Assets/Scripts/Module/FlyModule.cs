using UnityEngine;

namespace Mariolike
{
    public class FlyModule
    {
        private GameObject m_target = null;
        private float m_duration = 0f;
        private Vector3 m_destination = Vector3.zero;

        private Vector3 m_startPosition = Vector3.zero;
        private float m_timePassed = 0f;

        private bool m_flying = false;
        private bool m_done = false;

        public void setTarget(GameObject value)
        {
            m_target = value;
        }

        public void setDuration(float value)
        {
            m_duration = value;
        }

        public void setDestination(Vector3 value)
        {
            m_destination = value;
        }

        public void fly()
        {
            if (m_flying) return;
            m_flying = true;
            m_done = false;
            m_startPosition = m_target.transform.position;
            m_timePassed = 0f;
        }

        public bool isDone()
        {
            return m_done;
        }

        public void update(float deltaTime)
        {
            if (!m_flying) return;
            if (m_done) return;

            m_timePassed += deltaTime;
            float progress = Mathf.Min(1f, m_timePassed / m_duration);
            Vector3 pos = Vector3.Lerp(m_startPosition, m_destination, progress);
            m_target.transform.position = pos;
            if (m_timePassed >= m_duration)
            {
                m_done = true;
            }
        }
    }
}
