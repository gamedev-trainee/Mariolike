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

        protected override void onUpdateEntity(Entity entity)
        {
            JumpComponent jumpComponent = entity.getComponent<JumpComponent>();
            if (jumpComponent.iNextJumpDir > 0)
            {
                if (jumpComponent.mCurJumpDir == 0)
                {
                    jumpComponent.mCurJumpDir = 1;
                    jumpComponent.mCurJumpSpeed = jumpComponent.jumpSpeed;
                    AnimatorComponent animationPlayComponent = entity.getComponent<AnimatorComponent>();
                    if (animationPlayComponent != null)
                    {
                        animationPlayComponent.setParameter(JumpComponent.JumpStateParameter, (int)JumpComponent.JumpStates.Jumping);
                    }
                }
                jumpComponent.iNextJumpDir = 0;
            }
            if (jumpComponent.mCurJumpDir != 0)
            {
                PositionComponent positionComponent = entity.getComponent<PositionComponent>();
                positionComponent.addY(jumpComponent.mCurJumpSpeed * UnityEngine.Time.deltaTime);
                // 使用跳跃速度减去重力更新跳跃速度
                GravityComponent gravityComponent = entity.getComponent<GravityComponent>();
                jumpComponent.mCurJumpSpeed -= gravityComponent.gravity * UnityEngine.Time.deltaTime;
                if (jumpComponent.mCurJumpSpeed <= 0)
                {
                    jumpComponent.mCurJumpDir = -1;
                }
            }
        }
    }
}
