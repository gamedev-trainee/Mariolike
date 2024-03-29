﻿using UnityEngine;

namespace Mariolike
{
    public class HeroScript : CharacterScript
    {
        public float jumpStartSpeed = 0f;
        public Vector3 jumpAttackRangeCenter = Vector3.zero;
        public Vector3 jumpAttackRangeSize = Vector3.zero;
        public Vector3 objectAttackRangeCenter = Vector3.zero;
        public Vector3 objectAttackRangeSize = Vector3.zero;

        private JumpModule m_jumpModule = new JumpModule();
        private AttackModule m_jumpAttackModule = new AttackModule();
        private AttackModule m_objectAttackModule = new AttackModule();

        protected override void onStart()
        {
            base.onStart();

            m_jumpModule.setAnimator(animator);
            m_jumpModule.setGravity(gravity);
            m_jumpModule.setJumpSpeed(jumpStartSpeed);
            m_jumpAttackModule.setInstanceID(gameObject.GetInstanceID());
            m_jumpAttackModule.setTargetGroup(CharacterGroups.None);
            m_jumpAttackModule.setRangeOffset(jumpAttackRangeCenter);
            m_jumpAttackModule.setRangeSize(jumpAttackRangeSize);
            m_jumpAttackModule.setLayerMask(1 << (int)GameObjectLayers.Brick);
            m_objectAttackModule.setInstanceID(gameObject.GetInstanceID());
            m_objectAttackModule.setTargetGroup(CharacterGroups.None);
            m_objectAttackModule.setRangeOffset(objectAttackRangeCenter);
            m_objectAttackModule.setRangeSize(objectAttackRangeSize);
            m_objectAttackModule.setLayerMask(1 << (int)GameObjectLayers.Object);

            GameManager.Instance.registerHost(this);
        }

        protected override void onUpdatePosition(Vector3 pos, ref Vector3 offset)
        {
            base.onUpdatePosition(pos, ref offset);

            m_jumpModule.update(Time.deltaTime, ref offset);
        }

        protected override void onGroudHitTest(HitTestFlags flags, ref Vector3 offset)
        {
            base.onGroudHitTest(flags, ref offset);

            if (flags == HitTestFlags.HitGround)
            {
                m_jumpModule.stop();
                m_gravityModule.resetGravity();
            }
            else if (flags == HitTestFlags.OnGround)
            {
                if (!m_jumpModule.isJumping())
                {
                    offset.y = 0f;
                }
            }
        }

        protected override void onLateUpdate()
        {
            base.onLateUpdate();

            if (m_jumpModule.isJumpFalling())
            {
                m_attackModule.update(transform.position, this);
            }
            else if (m_jumpModule.isJumping())
            {
                if (m_jumpAttackModule.update(transform.position, this))
                {
                    m_jumpModule.reset();
                }
            }

            m_objectAttackModule.update(transform.position, this);
        }

        protected override void onScaleChanged(float scale)
        {
            base.onScaleChanged(scale);

            m_jumpAttackModule.setRangeScale(scale);
            m_objectAttackModule.setRangeScale(scale);
        }

        protected override bool canSetDead()
        {
            return !m_jumpModule.isJumping();
        }

        void OnGUI()
        {
            if (isDying())
            {
                return;
            }

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

        protected override void OnDrawGizmos()
        {
            base.OnDrawGizmos();

            Vector3 scale = transform.localScale;
            Color color = Gizmos.color;
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireCube(transform.position + jumpAttackRangeCenter * scale.x, jumpAttackRangeSize * scale.x);
            Gizmos.color = Color.green;
            Gizmos.DrawWireCube(transform.position + objectAttackRangeCenter * scale.x, objectAttackRangeSize * scale.x);
            Gizmos.color = color;
        }
    }
}

