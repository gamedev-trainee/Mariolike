using ECSlike;
using UnityEngine;

namespace Mariolike
{
    public class HitTestColliderComponent : IComponent, IComponentInitializer
    {
        // 属性

        [ConfigField(init = "GetComponent<UnityEngine.Collider>()")]
        public Collider collider = null;

        //
        public float radius = 0f;
        public float height = 0f;

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
