using ECSlike;
using UnityEngine;

namespace Mariolike
{
    public class HitTestGroundSystem : EntitySystem
    {
        protected sealed override System.Type[] getWantedTypes()
        {
            return new System.Type[]
            {
                typeof(HitTestColliderComponent),
                typeof(HitTestGroundComponent),
                typeof(PositionComponent),
            };
        }

        protected override System.Type[] getUnwantedTypes()
        {
            return new System.Type[]
            {
                typeof(DisposeComponent),
            };
        }

        protected override void onUpdateEntity(World world, int entity)
        {
            HitTestColliderComponent hitTestColliderComponent = world.getComponent<HitTestColliderComponent>(entity);
            HitTestGroundComponent hitTestGroundComponent = world.getComponent<HitTestGroundComponent>(entity);
            HitTestFlags oldFlags = hitTestGroundComponent.mHitFlags;
            PositionComponent positionComponent = world.getComponent<PositionComponent>(entity);
            if (positionComponent.offset.y <= 0f)
            {
                RaycastHit hit;
                float checkHeight = hitTestColliderComponent.height * 0.5f - positionComponent.offset.y;
                if (Physics.BoxCast(positionComponent.position + new Vector3(0f, hitTestColliderComponent.height, 0f), new Vector3(hitTestGroundComponent.footRadius, hitTestColliderComponent.height * 0.5f, hitTestGroundComponent.footRadius), Vector3.down, out hit, Quaternion.identity, checkHeight, hitTestGroundComponent.layerMask))
                {
                    if (oldFlags != HitTestFlags.OnGround)
                    {
                        hitTestGroundComponent.mHitFlags = HitTestFlags.OnGround;
                        positionComponent.offset.y = hit.point.y - positionComponent.position.y;
                    }
                    else
                    {
                        positionComponent.offset.y = 0;
                    }
                    return;
                }
            }
            if (oldFlags == HitTestFlags.OnGround)
            {
                hitTestGroundComponent.mHitFlags = HitTestFlags.None;
            }
        }
    }
}
