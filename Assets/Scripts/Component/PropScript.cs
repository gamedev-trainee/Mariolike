using UnityEngine;

namespace Mariolike
{
    public class PropScript : ObjectScript
    {
        public AttrTypes attr = AttrTypes.None;
        public AttrOperations operation = AttrOperations.Plus;
        public int value = 0;

        protected override void onStart()
        {
            base.onStart();

            if (movable)
            {
                m_moveModule.moveRandom();
            }
        }

        protected override void onGroudHitTest(HitTestFlags flags, ref Vector3 offset)
        {
            base.onGroudHitTest(flags, ref offset);

            if (flags == HitTestFlags.OnGround)
            {
                offset.y = 0f;
                m_gravityModule.resetGravity();
            }

            if (movable)
            {
                if (flags == HitTestFlags.None)
                {
                    m_moveModule.stop();
                }
                else
                {
                    if (!m_moveModule.isMoving())
                    {
                        m_moveModule.moveRandom();
                    }
                }
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

        protected override void onBeattackStart(ObjectScript attacker)
        {
            base.onBeattackStart(attacker);

            if (attr != AttrTypes.None)
            {
                int realValue = value;
                switch (operation)
                {
                    case AttrOperations.Minus:
                        {
                            realValue = -realValue;
                        }
                        break;
                }
                attacker.changeAttr(attr, realValue);
            }
        }
    }
}
