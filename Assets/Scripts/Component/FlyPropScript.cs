using UnityEngine;

namespace Mariolike
{
    public class FlyPropScript : PropScript
    {
        public float flyDuration = 0f;
        public float scaleTo = 0f;

        private float m_startZ = 0f;
        private FlyModule m_flyModule = new FlyModule();
        private ScaleModule m_scaleModule = new ScaleModule();

        protected override void onStart()
        {
            base.onStart();

            m_flyModule.setTarget(gameObject);
            m_flyModule.setDuration(flyDuration);
            m_scaleModule.setScaleSpeed((1f - scaleTo) / flyDuration);
        }

        protected override void onUpdate()
        {
            base.onUpdate();

            m_flyModule.setDestination(UIManager.Instance.getAttrLocWorldPosition(AttrTypes.Score, m_startZ));
            m_flyModule.update(Time.deltaTime);

            Vector3 scale = transform.localScale;
            if (m_scaleModule.update(Time.deltaTime, ref scale))
            {
                transform.localScale = scale;
            }
        }

        protected override void onKill(ObjectScript attacker)
        {
            base.onKill(attacker);

            m_startZ = gameObject.transform.position.z;
            m_flyModule.setDestination(UIManager.Instance.getAttrLocWorldPosition(AttrTypes.Score, m_startZ));
            m_flyModule.fly();
            m_scaleModule.scaleTo(scaleTo);
        }

        protected override bool canSetDead()
        {
            return m_flyModule.isDone();
        }
    }
}
