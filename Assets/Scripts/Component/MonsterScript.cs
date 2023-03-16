using UnityEngine;

namespace Mariolike
{
    public class MonsterScript : CharacterScript
    {
        protected override void onStart()
        {
            base.onStart();

            m_moveModule.moveRandom();
        }

        protected override void onUpdate(Vector3 pos, ref Vector3 offset)
        {
            m_moveModule.update(Time.deltaTime, ref offset);
            m_gravityModule.update(Time.deltaTime, ref offset);

            HitTestFlags flags = m_groundTestModule.update(pos, ref offset);
            if (flags == HitTestFlags.OnGround)
            {
                offset.y = 0f;
            }

            flags = m_wallTestModule.update(pos, m_moveModule.getMoveForward(), ref offset);
            if (flags == HitTestFlags.HitWall)
            {
                m_moveModule.reverse();
            }
        }

        protected override void onLateUpdate()
        {
            m_attackModule.update(transform.position);
        }
    }
}

