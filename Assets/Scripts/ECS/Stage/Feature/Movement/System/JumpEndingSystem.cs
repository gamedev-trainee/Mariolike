using ECSlike;

namespace Mariolike
{
    public class JumpEndingSystem : EntitySystem
    {
        protected sealed override System.Type[] getWantedTypes()
        {
            return new System.Type[]
            {
                typeof(JumpComponent),
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

        protected override void onUpdateEntity(Entity entity)
        {
            JumpComponent jumpComponent = entity.getComponent<JumpComponent>();
            if (jumpComponent.mCurJumpDir != 0)
            {
                HitTestGroundComponent hitTestGroundComponent = entity.getComponent<HitTestGroundComponent>();
                if (hitTestGroundComponent.mHitFlags == HitTestFlags.OnGround)
                {
                    jumpComponent.mCurJumpDir = 0;
                    AnimatorComponent animationPlayComponent = entity.getComponent<AnimatorComponent>();
                    if (animationPlayComponent != null)
                    {
                        animationPlayComponent.setParameter(JumpComponent.JumpStateParameter, (int)JumpComponent.JumpStates.None);
                    }
                }
            }
        }
    }
}
