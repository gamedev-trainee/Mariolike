using UnityEngine;

namespace Mariolike
{
    public class MoveModule
    {
        public enum MoveStates
        {
            None,
            Moving,
        }

        public static readonly string MoveStateParameter = "move_state";

        private Animator m_animator = null;
        private float m_moveSpeed = 0f;
        private float m_moveSpeedScale = 1f;

        private int m_lastMoveDir = 0;
        private int m_currentMoveDir = 0;
        private int m_moveForward = 0;
        private System.Action<int> m_moveForwardChangeCallback = null;

        public void setAnimator(Animator value)
        {
            m_animator = value;
        }

        public void setMoveSpeed(float value)
        {
            m_moveSpeed = value;
        }

        public void setMoveSpeedScale(float value)
        {
            m_moveSpeedScale = value;
        }

        public void setMoveForwardChangeCallback(System.Action<int> value)
        {
            m_moveForwardChangeCallback = value;
        }

        public int getMoveForward()
        {
            return m_moveForward;
        }

        public bool isMoving()
        {
            return m_currentMoveDir != 0;
        }

        public void moveRight()
        {
            m_currentMoveDir = 1;
            onMoveStart();
        }

        public void moveLeft()
        {
            m_currentMoveDir = -1;
            onMoveStart();
        }

        public void moveRandom()
        {
            m_currentMoveDir = Random.Range(0, 100) % 2 == 0 ? -1 : 1;
            onMoveStart();
        }

        public void reverse()
        {
            m_currentMoveDir = -m_currentMoveDir;
        }

        public void stop()
        {
            m_currentMoveDir = 0;
            onMoveEnd();
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
                offset.x += m_moveSpeed * m_moveSpeedScale * m_currentMoveDir * deltaTime;
            }
        }

        protected void onMoveStart()
        {
            if (m_animator == null) return;
            if (m_animator.GetInteger(MoveStateParameter) > (int)MoveStates.None) return;
            m_animator.SetInteger(MoveStateParameter, (int)MoveStates.Moving);
        }

        protected void onMoveEnd()
        {
            if (m_animator == null) return;
            if (m_animator.GetInteger(MoveStateParameter) <= (int)MoveStates.None) return;
            m_animator.SetInteger(MoveStateParameter, (int)MoveStates.None);
        }
    }
}
