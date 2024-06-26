﻿using ECSlike;

namespace Mariolike
{
    public class MoveSystem : EntitySystem
    {
        protected sealed override System.Type[] getWantedTypes()
        {
            return new System.Type[]
            {
                typeof(MoveComponent),
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
            MoveComponent moveComponent = entity.getComponent<MoveComponent>();
            if (moveComponent.mCurMoveDir != moveComponent.iNextMoveDir)
            {
                // 当前移动方向与下一个移动方向不一致时，更新当前移动方向
                bool isMoveStart = moveComponent.mCurMoveDir == 0;
                bool isMoveEnd = moveComponent.mCurMoveDir != 0 && moveComponent.iNextMoveDir == 0;
                moveComponent.mCurMoveDir = moveComponent.iNextMoveDir;
                if (moveComponent.mCurMoveDir != 0)
                {
                    // 当前移动方向变更后不为0时，更新一下角色朝向
                    RotationComponent rotationComponent = entity.getComponent<RotationComponent>();
                    if (rotationComponent != null)
                    {
                        rotationComponent.setDirection(moveComponent.mCurMoveDir);
                    }
                }
                if (isMoveStart)
                {
                    AnimatorComponent animatorComponent = entity.getComponent<AnimatorComponent>();
                    if (animatorComponent != null)
                    {
                        animatorComponent.setParameter(MoveComponent.MoveStateParameter, (int)MoveComponent.MoveStates.Moving);
                    }
                }
                else if (isMoveEnd)
                {
                    AnimatorComponent animatorComponent = entity.getComponent<AnimatorComponent>();
                    if (animatorComponent != null)
                    {
                        animatorComponent.setParameter(MoveComponent.MoveStateParameter, (int)MoveComponent.MoveStates.None);
                    }
                }
            }
            if (moveComponent.mCurMoveDir != 0)
            {
                PositionComponent positionComponent = entity.getComponent<PositionComponent>();
                positionComponent.addX(moveComponent.moveSpeed * moveComponent.mCurMoveDir * UnityEngine.Time.deltaTime);
            }
        }
    }
}
