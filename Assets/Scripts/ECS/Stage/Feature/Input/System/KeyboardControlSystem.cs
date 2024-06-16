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

        protected override void onUpdateEntity(World world, int entity)
        {
            DeathComponent deathComponent = world.getComponent<DeathComponent>(entity);
            if (deathComponent != null && deathComponent.isDying())
            {
                return;
            }
            KeyboardControlComponent keyboardComponent = world.getComponent<KeyboardControlComponent>(entity);
            MoveComponent moveComponent = world.getComponent<MoveComponent>(entity);
            JumpComponent jumpComponent = world.getComponent<JumpComponent>(entity);
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
