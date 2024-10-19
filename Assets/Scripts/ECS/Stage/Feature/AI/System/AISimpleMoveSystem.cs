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

        protected override void onUpdateEntity(Entity entity)
        {
            DeathComponent deathComponent = entity.getComponent<DeathComponent>();
            if (deathComponent != null && deathComponent.isDying())
            {
                return;
            }
            MoveComponent moveComponent = entity.getComponent<MoveComponent>();
            if (!moveComponent.isMoving())
            {
                moveComponent.startMove(Random.Range(0, 100) % 2 == 0 ? -1 : 1);
            }
            else
            {
                HitTestComponent hitTestComponent = entity.getComponent<HitTestComponent>();
                if (hitTestComponent != null)
                {
                    if (hitTestComponent.isHitWall())
                    {
                        moveComponent.turnAround();
                    }
                }
            }
        }
    }
}
