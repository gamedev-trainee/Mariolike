using UnityEngine;

namespace Mariolike
{
    public class HeroScriptV4 : MonoBehaviour
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

        private bool m_jumpFalling = false;
        private bool m_dead = false;

        void Update()
        {
            if (m_dead)
            {
                GameObject.Destroy(gameObject);
                return;
            }

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

            m_jumpFalling = m_jumpDir == 1 && offset.y < 0f;

            if (offset.y < 0f)
            {
                float checkHeight = height * 0.5f - offset.y;
                if (Physics.BoxCast(pos + new Vector3(0f, height, 0f), new Vector3(footRadius, height * 0.5f, footRadius), Vector3.down, out hit, Quaternion.identity, checkHeight, 1 << (int)GameObjectLayers.Default))
                {
                    if (hit.collider.gameObject != gameObject)
                    {
                        if (hit.point.y <= pos.y + stepOffset)
                        {
                            nextPos.y = hit.point.y;
                            offset.y = nextPos.y - pos.y;
                            m_jumpDir = 0;
                        }
                        else
                        {
                            if (m_jumpDir == 0)
                            {
                                offset.y = 0f;
                            }
                        }
                    }
                }
            }

            if (offset.sqrMagnitude > 0)
            {
                float checkDistance = offset.x * m_lookDir + radius;
                Vector3 pointBottom = pos + new Vector3(0f, radius, 0f) + new Vector3(-m_lookDir * radius, 0f, 0f);
                Vector3 pointTop = pos + new Vector3(0f, radius + height, 0f) + new Vector3(-m_lookDir * radius, 0f, 0f);
                RaycastHit[] hits = Physics.CapsuleCastAll(pointBottom, pointTop, radius, new Vector3(m_lookDir, 0f, 0f), checkDistance, 1 << (int)GameObjectLayers.Default);
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

        void LateUpdate()
        {
            if (m_dead)
            {
                return;
            }

            if (m_jumpFalling)
            {
                Vector3 pos = transform.position;
                Collider[] colliders = Physics.OverlapBox(pos, new Vector3(radius, height * 0.2f, radius), Quaternion.identity, 1 << (int)GameObjectLayers.Monster);
                if (colliders != null && colliders.Length > 0)
                {
                    MonsterScript monsterScript;
                    int count = colliders.Length;
                    for (int i = 0; i < count; i++)
                    {
                        monsterScript = colliders[i].GetComponent<MonsterScript>();
                        if (monsterScript == null) continue;
                        monsterScript.kill();
                    }
                }
            }
        }

        public bool isJumping()
        {
            return m_jumpDir == 0;
        }

        public void kill()
        {
            m_dead = true;
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

