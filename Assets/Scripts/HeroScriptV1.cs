using UnityEngine;

namespace Mariolike
{
    public class HeroScriptV1 : MonoBehaviour
    {
        public float moveSpeed = 0f;
        public float jumpStartSpeed = 0f;
        public float gravity = 0f;

        private int m_lastMoveDir = 0;
        private int m_moveDir = 0;

        private int m_jumpDir = 0;
        private float m_jumpSpeed = 0f;

        void Update()
        {
            if (m_moveDir != 0)
            {
                if (m_lastMoveDir != m_moveDir)
                {
                    m_lastMoveDir = m_moveDir;
                    Vector3 eulerAngles = transform.eulerAngles;
                    eulerAngles.y = m_lastMoveDir < 0 ? 180 : 0;
                    transform.eulerAngles = eulerAngles;
                }
                Vector3 pos = transform.position;
                pos.x += moveSpeed * m_moveDir * Time.deltaTime;
                transform.position = pos;
            }
            if (m_jumpDir != 0)
            {
                Vector3 pos = transform.position;
                pos.y -= gravity * Time.deltaTime;
                pos.y += m_jumpSpeed * m_jumpDir * Time.deltaTime;
                if (pos.y <= 0f)
                {
                    pos.y = 0;
                    m_jumpDir = 0;
                }
                transform.position = pos;
                m_jumpSpeed -= gravity * Time.deltaTime;
            }
        }

        void OnGUI()
        {
            if (Input.GetKey(KeyCode.LeftArrow))
            {
                m_moveDir = -1;
            }
            else if (Input.GetKey(KeyCode.RightArrow))
            {
                m_moveDir = 1;
            }
            else
            {
                m_moveDir = 0;
            }
            if (m_jumpDir == 0)
            {
                if (Input.GetKeyDown(KeyCode.UpArrow))
                {
                    m_jumpDir = 1;
                    m_jumpSpeed = jumpStartSpeed;
                }
            }
        }
    }
}
