using UnityEngine;

namespace Mariolike
{
    public abstract class CharacterScript : ObjectScript
    {
        public CharacterGroups group = CharacterGroups.A;
        public Vector3 attackRangeCenter = Vector3.zero;
        public Vector3 attackRangeSize = Vector3.zero;
        public float scaleSpeed = 0f;

        protected AttackModule m_attackModule = new AttackModule();
        protected ScaleModule m_scaleModule = new ScaleModule();

        protected override void onStart()
        {
            base.onStart();

            m_attackModule.setInstanceID(gameObject.GetInstanceID());
            m_attackModule.setTargetGroup(group == CharacterGroups.A ? CharacterGroups.B : CharacterGroups.A);
            m_attackModule.setRangeOffset(attackRangeCenter);
            m_attackModule.setRangeSize(attackRangeSize);
            m_attackModule.setLayerMask(1 << (int)GameObjectLayers.Character);
            m_scaleModule.setScaleSpeed(scaleSpeed);
        }

        protected sealed override void onUpdate()
        {
            base.onUpdate();

            Vector3 scale = transform.localScale;
            if (m_scaleModule.update(Time.deltaTime, ref scale))
            {
                transform.localScale = scale;
                onScaleChanged(scale.x);
            }
        }

        protected virtual void onScaleChanged(float scale)
        {
            m_attackModule.setRangeScale(scale);
        }

        protected override void onAttrChanged(AttrTypes type, int value)
        {
            base.onAttrChanged(type, value);

            switch (type)
            {
                case AttrTypes.ScaleHP:
                    {
                        if (value > 0)
                        {
                            m_scaleModule.scaleTo(2f);
                        }
                        else if (value < 0)
                        {
                            m_scaleModule.scaleTo(1f);
                        }
                    }
                    break;
            }
        }

        protected virtual void OnDrawGizmos()
        {
            Vector3 scale = transform.localScale;
            Color color = Gizmos.color;
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(transform.position + attackRangeCenter * scale.x, attackRangeSize * scale.x);
            Gizmos.color = color;
        }
    }
}

