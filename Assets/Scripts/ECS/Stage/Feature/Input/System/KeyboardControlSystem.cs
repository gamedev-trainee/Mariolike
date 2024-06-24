using ECSlike;
using UnityEngine;

namespace Mariolike
{
    public class KeyboardControlSystem : EntitySystem
    {
        protected sealed override System.Type[] getWantedTypes()
        {
            return new System.Type[]
            {
                typeof(KeyboardControlComponent),
                typeof(MoveComponent),
                typeof(JumpComponent),
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
            KeyboardControlComponent keyboardComponent = entity.getComponent<KeyboardControlComponent>();
            MoveComponent moveComponent = entity.getComponent<MoveComponent>();
            JumpComponent jumpComponent = entity.getComponent<JumpComponent>();
            if (Input.GetKey(keyboardComponent.moveLeftKey))
            {
                moveComponent.startMoveLeft();
            }
            else if (Input.GetKey(keyboardComponent.moveRightKey))
            {
                moveComponent.startMoveRight();
            }
            else
            {
                moveComponent.stopMove();
            }
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                jumpComponent.startJump();
            }
        }
    }
}
