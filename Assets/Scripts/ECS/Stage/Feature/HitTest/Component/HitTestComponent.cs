using ECSlike;
using UnityEngine;

namespace Mariolike
{
    public class HitTestComponent : IComponent, IComponentInitializer
    {
        // 属性

        [ConfigField(init = "GetComponent<UnityEngine.Collider>()")]
        public Collider collider = null;
        [ConfigField]
        public float footRadius = 0f;
        [ConfigField]
        public float stepOffset = 0.1f;
        [ConfigField]
        public LayerMask layerMask = 1 << (int)GameObjectLayers.Default | 1 << (int)GameObjectLayers.Brick;

        //
        public float radius = 0f;
        public float height = 0f;

        public HitTestFlags mHitFlags = HitTestFlags.None;
        public HitTestFlags mLastHitFlags = HitTestFlags.None;

        public bool isHitWall()
        {
            return (mHitFlags & HitTestFlags.HitWall) == HitTestFlags.HitWall;
        }

        public bool isHitGround()
        {
            return (mHitFlags & HitTestFlags.HitGround) == HitTestFlags.HitGround;
        }

        // IComponentInitializer

        public void initComponent()
        {
            if (collider != null)
            {
                if (collider is CapsuleCollider)
                {
                    radius = (collider as CapsuleCollider).radius;
                    height = (collider as CapsuleCollider).height;
                }
                else if (collider is SphereCollider)
                {
                    radius = (collider as SphereCollider).radius;
                    height = (collider as SphereCollider).radius;
                }
                else if (collider is BoxCollider)
                {
                    radius = Mathf.Max((collider as BoxCollider).size.x, (collider as BoxCollider).size.y) * 0.5f;
                    height = (collider as BoxCollider).size.y;
                }
            }
        }
    }
}
