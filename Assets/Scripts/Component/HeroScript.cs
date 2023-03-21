using UnityEngine;

namespace Mariolike
{
    public class HeroScript : CharacterScript
    {
        public float jumpStartSpeed = 0f;

        private JumpModule m_jumpModule = new JumpModule();

        protected override void onStart()
        {
            m_jumpModule.setGravity(gravity);
            m_jumpModule.setJumpSpeed(jumpStartSpeed);
        }

        protected override void onUpdate(Vector3 pos, ref Vector3 offset)
        {
            m_moveModule.update(Time.deltaTime, ref offset);
            m_gravityModule.update(Time.deltaTime, ref offset);
            m_jumpModule.update(Time.deltaTime, ref offset);

            HitTestFlags flags = m_groundTestModule.update(pos, ref offset);
            if (flags == HitTestFlags.HitGround)
            {
                m_jumpModule.stop();
            }
            else if (flags == HitTestFlags.OnGround)
            {
                if (!m_jumpModule.isJumping())
                {
                    offset.y = 0f;
                }
            }

            m_wallTestModule.update(pos, m_moveModule.getMoveForward(), ref offset);
        }

        protected override void onLateUpdate()
        {
            if (m_jumpModule.isJumpFalling())
            {
                m_attackModule.update(transform.position);
            }
        }

        void OnGUI()
        {
            if (Input.GetKey(KeyCode.LeftArrow))
            {
                m_moveModule.moveLeft();
            }
            else if (Input.GetKey(KeyCode.RightArrow))
            {
                m_moveModule.moveRight();
            }
            else
            {
                m_moveModule.stop();
            }
            if (!m_jumpModule.isJumping())
            {
                if (Input.GetKeyDown(KeyCode.UpArrow))
                {
                    m_jumpModule.jump();
                }
            }
        }
    }
}

