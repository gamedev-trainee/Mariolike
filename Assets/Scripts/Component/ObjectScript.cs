using System.Collections.Generic;
using UnityEngine;

namespace Mariolike
{
    public class ObjectScript : MonoBehaviour
    {
        public Animator animator = null;
        public List<AttrInfo> attrs = new List<AttrInfo>();
        public bool movable = true;
        public float moveSpeed = 0f;
        public float gravity = 0f;
        public float radius = 0f;
        public float height = 0f;
        public float footRadius = 0f;
        public float stepOffset = 0f;

        private bool m_dead = false;
        private bool m_dying = false;

        private bool m_beattacking = false;
        private Dictionary<AttrTypes, int> m_attrs = new Dictionary<AttrTypes, int>();

        protected MoveModule m_moveModule = new MoveModule();
        protected GravityModule m_gravityModule = new GravityModule();
        protected GroundTestModule m_groundTestModule = new GroundTestModule();
        protected WallTestModule m_wallTestModule = new WallTestModule();
        protected BeattackModule m_beattackModule = new BeattackModule();
        protected DeathModule m_deathModule = new DeathModule();

        private void Start()
        {
            int count = attrs.Count;
            for (int i = 0; i < count; i++)
            {
                m_attrs[attrs[i].type] = attrs[i].value;
            }
            if (m_attrs.ContainsKey(AttrTypes.MaxHP))
            {
                m_attrs[AttrTypes.HP] = m_attrs[AttrTypes.MaxHP];
            }

            m_moveModule.setAnimator(animator);
            m_moveModule.setMoveSpeed(moveSpeed);
            m_moveModule.setMoveForwardChangeCallback((int moveForward) =>
            {
                Vector3 eulerAngles = transform.eulerAngles;
                eulerAngles.y = moveForward < 0 ? 180 : 0;
                transform.eulerAngles = eulerAngles;
            });
            m_gravityModule.setGravity(gravity);
            m_groundTestModule.setInstanceID(gameObject.GetInstanceID());
            m_groundTestModule.setRadius(footRadius);
            m_groundTestModule.setHeight(height);
            m_groundTestModule.setStepOffset(stepOffset);
            m_groundTestModule.setLayerMask(1 << (int)GameObjectLayers.Default | 1 << (int)GameObjectLayers.Brick);
            m_wallTestModule.setInstanceID(gameObject.GetInstanceID());
            m_wallTestModule.setRadius(radius);
            m_wallTestModule.setHeight(height);
            m_wallTestModule.setStepOffset(stepOffset);
            m_wallTestModule.setLayerMask(1 << (int)GameObjectLayers.Default | 1 << (int)GameObjectLayers.Brick);
            m_beattackModule.setAnimator(animator);
            m_deathModule.setAnimator(animator);

            onStart();
        }

        protected virtual void onStart()
        {

        }

        void Update()
        {
            if (m_dead)
            {
                GameObject.Destroy(gameObject);
                return;
            }

            onUpdate();
        }

        protected virtual void onUpdate()
        {
            if (!movable)
            {
                return;
            }

            Vector3 offset = Vector3.zero;
            Vector3 pos = transform.position;

            onUpdatePosition(pos, ref offset);
            onHitTest(pos, ref offset);

            if (offset.sqrMagnitude > 0)
            {
                pos += offset;
                transform.position = pos;
            }
        }

        protected virtual void onUpdatePosition(Vector3 pos, ref Vector3 offset)
        {
            m_moveModule.update(Time.deltaTime, ref offset);
            m_gravityModule.update(Time.deltaTime, ref offset);
        }

        protected virtual void onHitTest(Vector3 pos, ref Vector3 offset)
        {
            HitTestFlags flags = m_groundTestModule.update(pos, ref offset);
            onGroudHitTest(flags, ref offset);

            flags = m_wallTestModule.update(pos, m_moveModule.getMoveForward(), ref offset);
            onWallHitTest(flags, ref offset);
        }

        protected virtual void onGroudHitTest(HitTestFlags flags, ref Vector3 offset)
        {

        }

        protected virtual void onWallHitTest(HitTestFlags flags, ref Vector3 offset)
        {

        }

        void LateUpdate()
        {
            if (m_dead)
            {
                return;
            }

            if (m_dying)
            {
                if (m_deathModule.update())
                {
                    if (canSetDead())
                    {
                        setDead();
                    }
                }
            }
            else if (m_beattacking)
            {
                if (m_beattackModule.update())
                {
                    onBeattackEnd();
                }
            }
            else
            {
                onLateUpdate();
            }
        }

        protected virtual void onLateUpdate()
        {

        }

        public int getAttr(AttrTypes type)
        {
            int value;
            if (m_attrs.TryGetValue(type, out value))
            {
                return value;
            }
            return 0;
        }

        public void setAttr(AttrTypes type, int value)
        {
            m_attrs[type] = value;
        }

        public void changeAttr(AttrTypes type, int value)
        {
            if (m_attrs.ContainsKey(type))
            {
                m_attrs[type] += value;
            }
            else
            {
                m_attrs[type] = value;
            }
            onAttrChanged(type, value);
        }

        protected virtual void onAttrChanged(AttrTypes type, int value)
        {

        }

        public void beattack(ObjectScript attacker)
        {
            if (m_beattacking) return;
            m_beattacking = true;
            onBeattackStart(attacker);
        }

        protected virtual void onBeattackStart(ObjectScript attacker)
        {
            if (getAttr(AttrTypes.ScaleHP) > 0)
            {
                changeAttr(AttrTypes.ScaleHP, -1);
            }
            else if (getAttr(AttrTypes.MaxHP) > 0)
            {
                changeAttr(AttrTypes.HP, -1);
                if (getAttr(AttrTypes.HP) <= 0)
                {
                    kill(attacker);
                    return;
                }
            }
            m_beattackModule.beattack();
            m_moveModule.stop();
        }

        protected virtual void onBeattackEnd()
        {
            m_beattacking = false;
        }

        public void kill(ObjectScript attacker)
        {
            if (m_dying) return;
            m_dying = true;
            onKill(attacker);
        }

        protected virtual void onKill(ObjectScript attacker)
        {
            m_deathModule.kill();
            m_moveModule.stop();
        }

        public bool isDying()
        {
            return m_dying;
        }

        protected void setDead()
        {
            m_dead = true;
        }

        protected virtual bool canSetDead()
        {
            return true;
        }
    }
}
