using ECSlike;
using UnityEngine;

namespace Mariolike
{
    public class AISimpleMoveSystem : EntitySystem
    {
        protected sealed override System.Type[] getWantedTypes()
        {
            return new System.Type[]
            {
                typeof(AISimpleMoveComponent),
                typeof(MoveComponent),
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
            DeathComponent deathComponent = world.getComponent<DeathComponent>(entity);
            if (deathComponent != null && deathComponent.isDying())
            {
                return;
            }
            MoveComponent moveComponent = world.getComponent<MoveComponent>(entity);
            if (!moveComponent.isMoving())
            {
                moveComponent.startMove(Random.Range(0, 100) % 2 == 0 ? -1 : 1);
            }
            else
            {
                HitTestWallComponent hitTestComponent = world.getComponent<HitTestWallComponent>(entity);
                if (hitTestComponent != null)
                {
                    if (hitTestComponent.mHitFlags != HitTestFlags.None)
                    {
                        moveComponent.turnAround();
                    }
                }
            }
        }
    }
}
