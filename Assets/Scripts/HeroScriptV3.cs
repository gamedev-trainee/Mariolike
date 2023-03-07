using UnityEngine;

namespace Mariolike
{
    public class HeroScriptV3 : MonoBehaviour
    {
        public float moveSpeed = 0f;
        public float jumpStartSpeed = 0f;
        public float gravity = 0f;
        public float radius = 0f;
        public float height = 0f;
        public float footRadius = 0f;
        public float stepOffset = 0f;

        private int m_lastMoveDir = 0;
        private int m_moveDir = 0;
        private int m_lookDir = 0;

        private int m_jumpDir = 0;
        private float m_jumpSpeed = 0f;

        void Update()
        {
            Vector3 offset = Vector3.zero;
            Vector3 pos = transform.position;

            if (m_moveDir != 0)
            {
                if (m_lastMoveDir != m_moveDir)
                {
                    m_lastMoveDir = m_moveDir;
                    m_lookDir = m_moveDir;
                    Vector3 eulerAngles = transform.eulerAngles;
                    eulerAngles.y = m_lastMoveDir < 0 ? 180 : 0;
                    transform.eulerAngles = eulerAngles;
                }
                offset.x += moveSpeed * m_moveDir * Time.deltaTime;
            }

            if (m_jumpDir != 0)
            {
                offset.y += m_jumpSpeed * m_jumpDir * Time.deltaTime;
                m_jumpSpeed -= gravity * Time.deltaTime;
            }

            offset.y -= gravity * Time.deltaTime;

            Vector3 nextPos = pos + offset;
            RaycastHit hit;

            if (offset.y < 0f)
            {
                float checkHeight = height * 0.5f - offset.y;
                if (Physics.BoxCast(pos + new Vector3(0f, height, 0f), new Vector3(footRadius, height * 0.5f, footRadius), Vector3.down, out hit, Quaternion.identity, checkHeight))
                {
                    if (hit.collider.gameObject != gameObject)
                    {
                        if (hit.point.y <= pos.y + stepOffset)
                        {
                            nextPos.y = hit.point.y;
                            offset.y = nextPos.y - pos.y;
                            m_jumpDir = 0;
                        }
                    }
                }
            }

            if (offset.sqrMagnitude > 0)
            {
                float checkDistance = offset.x * m_lookDir + radius;
                RaycastHit[] hits = Physics.BoxCastAll(pos + new Vector3(-radius * m_lookDir, height * 0.5f, 0f), new Vector3(radius, height * 0.5f, radius), new Vector3(m_lookDir, 0f, 0f), Quaternion.identity, checkDistance);
                if (hits != null && hits.Length > 0)
                {
                    int count = hits.Length;
                    for (int i = 0; i < count; i++)
                    {
                        if (hits[i].collider.gameObject == gameObject) continue;
                        if (hits[i].point.y > nextPos.y + stepOffset)
                        {
                            nextPos.x = hits[i].point.x - radius * m_lookDir;
                            offset.x = nextPos.x - pos.x;
                            break;
                        }
                    }
                }
            }

            if (offset.sqrMagnitude > 0)
            {
                pos += offset;
                transform.position = pos;
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

