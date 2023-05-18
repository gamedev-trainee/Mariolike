using UnityEngine;

namespace Mariolike
{
    public class MonsterScript : CharacterScript
    {
        public GameObject beattackDrop = null;
        public int beattackDropCount = 0;

        public float deathMoveSpeedScale = 1f;

        protected override void onStart()
        {
            base.onStart();

            m_moveModule.moveRandom();
        }

        protected override void onGroudHitTest(HitTestFlags flags, ref Vector3 offset)
        {
            base.onGroudHitTest(flags, ref offset);

            if (flags == HitTestFlags.OnGround)
            {
                offset.y = 0f;
                m_gravityModule.resetGravity();
            }
        }

        protected override void onWallHitTest(HitTestFlags flags, ref Vector3 offset)
        {
            base.onWallHitTest(flags, ref offset);

            if (flags == HitTestFlags.HitWall)
            {
                if (!isDying())
                {
                    m_moveModule.reverse();
                }
            }
        }

        protected override void onLateUpdate()
        {
            base.onLateUpdate();

            m_attackModule.update(transform.position, this);
        }

        protected override void onBeattackStart(ObjectScript attacker)
        {
            if (beattackDrop != null)
            {
                GameObject propInst;
                for (int i = 0; i < beattackDropCount; i++)
                {
                    propInst = GameObject.Instantiate(beattackDrop);
                    propInst.transform.position = transform.position;
                    propInst.transform.localScale = Vector3.one;
                    propInst.transform.localRotation = Quaternion.identity;
                    PropScript propScript = propInst.GetComponent<PropScript>();
                    if (propScript != null)
                    {
                        propScript.beattack(attacker);
                    }
                }
            }
            base.onBeattackStart(attacker);
        }

        protected override void onKill(ObjectScript attacker)
        {
            base.onKill(attacker);

            m_moveModule.setMoveSpeedScale(deathMoveSpeedScale);
            m_moveModule.setMoveForwardChangeCallback(null);
            if (m_moveModule.getMoveForward() > 0)
            {
                m_moveModule.moveLeft();
            }
            else
            {
                m_moveModule.moveRight();
            }
        }
    }
}

