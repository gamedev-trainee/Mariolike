using ECSlike;

namespace Mariolike
{
    public class JumpSystem : EntitySystem
    {
        protected sealed override System.Type[] getWantedTypes()
        {
            return new System.Type[]
            {
                typeof(JumpComponent),
                typeof(GravityComponent),
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
            if (jumpComponent.iNextJumpDir > 0)
            {
                if (jumpComponent.mCurJumpDir == 0)
                {
                    jumpComponent.mCurJumpDir = 1;
                    jumpComponent.mCurJumpSpeed = jumpComponent.jumpSpeed;
                    AnimatorComponent animationPlayComponent = world.getComponent<AnimatorComponent>(entity);
                    if (animationPlayComponent != null)
                    {
                        animationPlayComponent.setParameter(JumpComponent.JumpStateParameter, (int)JumpComponent.JumpStates.Jumping);
                    }
                }
                jumpComponent.iNextJumpDir = 0;
            }
            if (jumpComponent.mCurJumpDir != 0)
            {
                PositionComponent positionComponent = world.getComponent<PositionComponent>(entity);
                positionComponent.addY(jumpComponent.mCurJumpSpeed * UnityEngine.Time.deltaTime);
                // 使用跳跃速度减去重力更新跳跃速度
                GravityComponent gravityComponent = world.getComponent<GravityComponent>(entity);
                jumpComponent.mCurJumpSpeed -= gravityComponent.gravity * UnityEngine.Time.deltaTime;
                if (jumpComponent.mCurJumpSpeed <= 0)
                {
                    jumpComponent.mCurJumpDir = -1;
                }
            }
        }
    }
}
