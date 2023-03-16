using UnityEngine;

namespace Mariolike
{
    public abstract class CharacterScript : MonoBehaviour
    {
        public float moveSpeed = 0f;
        public float gravity = 0f;
        public float radius = 0f;
        public float height = 0f;
        public float footRadius = 0f;
        public float stepOffset = 0f;
        public CharacterGroups group = CharacterGroups.A;
        public Vector3 attackRangeCenter = Vector3.zero;
        public Vector3 attackRangeSize = Vector3.zero;

        private bool m_dead = false;

        protected MoveModule m_moveModule = new MoveModule();
        protected GravityModule m_gravityModule = new GravityModule();
        protected GroundTestModule m_groundTestModule = new GroundTestModule();
        protected WallTestModule m_wallTestModule = new WallTestModule();
        protected AttackModule m_attackModule = new AttackModule();

        private void Start()
        {
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
            m_groundTestModule.setLayerMask(1 << (int)GameObjectLayers.Default);
            m_wallTestModule.setInstanceID(gameObject.GetInstanceID());
            m_wallTestModule.setRadius(radius);
            m_wallTestModule.setHeight(height);
            m_wallTestModule.setStepOffset(stepOffset);
            m_wallTestModule.setLayerMask(1 << (int)GameObjectLayers.Default);
            m_attackModule.setInstanceID(gameObject.GetInstanceID());
            m_attackModule.setTargetGroup(group == CharacterGroups.A ? CharacterGroups.B : CharacterGroups.A);
            m_attackModule.setRangeOffset(attackRangeCenter);
            m_attackModule.setRangeSize(attackRangeSize);
            m_attackModule.setLayerMask(1 << (int)GameObjectLayers.Character);

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

            Vector3 offset = Vector3.zero;
            Vector3 pos = transform.position;

            onUpdate(pos, ref offset);

            if (offset.sqrMagnitude > 0)
            {
                pos += offset;
                transform.position = pos;
            }
        }

        protected virtual void onUpdate(Vector3 pos, ref Vector3 offset)
        {

        }

        void LateUpdate()
        {
            if (m_dead)
            {
                return;
            }

            onLateUpdate();
        }

        protected virtual void onLateUpdate()
        {

        }

        public void kill()
        {
            m_dead = true;
        }

        private void OnDrawGizmos()
        {
            Gizmos.DrawWireCube(transform.position + attackRangeCenter, attackRangeSize);
        }
    }
}

