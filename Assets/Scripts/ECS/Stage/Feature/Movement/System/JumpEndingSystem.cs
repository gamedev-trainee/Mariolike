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

        protected override void onUpdateEntity(World world, int entity)
        {
            JumpComponent jumpComponent = world.getComponent<JumpComponent>(entity);
            if (jumpComponent.mCurJumpDir != 0)
            {
                HitTestGroundComponent hitTestGroundComponent = world.getComponent<HitTestGroundComponent>(entity);
                if (hitTestGroundComponent.mHitFlags == HitTestFlags.OnGround)
                {
                    jumpComponent.mCurJumpDir = 0;
                    AnimatorComponent animationPlayComponent = world.getComponent<AnimatorComponent>(entity);
                    if (animationPlayComponent != null)
                    {
                        animationPlayComponent.setParameter(JumpComponent.JumpStateParameter, (int)JumpComponent.JumpStates.None);
                    }
                }
            }
        }
    }
}
