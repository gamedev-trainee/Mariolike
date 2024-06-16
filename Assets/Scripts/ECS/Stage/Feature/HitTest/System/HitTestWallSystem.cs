using ECSlike;
using UnityEngine;

namespace Mariolike
{
    public class HitTestWallSystem : EntitySystem
    {
        protected sealed override System.Type[] getWantedTypes()
        {
            return new System.Type[]
            {
                typeof(HitTestColliderComponent),
                typeof(HitTestWallComponent),
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
            HitTestWallComponent hitTestWallComponent = world.getComponent<HitTestWallComponent>(entity);
            hitTestWallComponent.mHitFlags = HitTestFlags.None;
            PositionComponent positionComponent = world.getComponent<PositionComponent>(entity);
            if (positionComponent.offset.x != 0f)
            {
                int moveForward = positionComponent.offset.x > 0 ? 1 : -1;
                float checkDistance = Mathf.Abs(positionComponent.offset.x) + hitTestColliderComponent.radius;
                Vector3 pointBottom = positionComponent.position + new Vector3(0f, hitTestColliderComponent.radius, 0f) + new Vector3(-moveForward * hitTestColliderComponent.radius, 0f, 0f);
                Vector3 pointTop = positionComponent.position + new Vector3(0f, hitTestColliderComponent.height - hitTestColliderComponent.radius, 0f) + new Vector3(-moveForward * hitTestColliderComponent.radius, 0f, 0f);
                RaycastHit[] hits = Physics.CapsuleCastAll(pointBottom, pointTop, hitTestColliderComponent.radius, new Vector3(moveForward, 0f, 0f), checkDistance, hitTestWallComponent.layerMask);
                if (hits != null && hits.Length > 0)
                {
                    float hitY;
                    int count = hits.Length;
                    for (int i = 0; i < count; i++)
                    {
                        if (moveForward > 0 && hits[i].point.x < positionComponent.position.x) continue;
                        if (moveForward < 0 && hits[i].point.x > positionComponent.position.x) continue;
                        hitY = Mathf.Round(hits[i].point.y * 1000f) * 0.001f;
                        if (hitY > positionComponent.position.y + positionComponent.offset.y + hitTestWallComponent.stepOffset)
                        {
                            float nextX = hits[i].point.x - hitTestColliderComponent.radius * moveForward;
                            positionComponent.offset.x = nextX - positionComponent.position.x;
                            hitTestWallComponent.mHitFlags = HitTestFlags.HitWall;
                            break;
                        }
                    }
                }
            }
        }
    }
}
