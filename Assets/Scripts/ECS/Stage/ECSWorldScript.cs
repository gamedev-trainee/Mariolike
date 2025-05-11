using UnityEngine;

namespace Mariolike
{
    public class ECSWorldScript : MonoBehaviour
    {
        private void Start()
        {
            ECSWorld.Instance.addSystem<KeyboardControlSystem>();
            ECSWorld.Instance.addSystem<AISimpleMoveSystem>();
            //
            ECSWorld.Instance.addSystem<MoveSystem>();
            ECSWorld.Instance.addSystem<JumpSystem>();
            ECSWorld.Instance.addSystem<GravitySystem>();
            //
            ECSWorld.Instance.addSystem<HitTestSystem>();
            //
            ECSWorld.Instance.addSystem<JumpEndingSystem>();
            ECSWorld.Instance.addSystem<GravityEndingSystem>();
            //
            ECSWorld.Instance.addSystem<PositionSystem>();
            ECSWorld.Instance.addSystem<RotationSystem>();
            //
            ECSWorld.Instance.addSystem<AttackSystem>();
            ECSWorld.Instance.addSystem<BeattackSystem>();
            ECSWorld.Instance.addSystem<DeathSystem>();
            //
            ECSWorld.Instance.addSystem<MotionPlaySystem>();
            //
            ECSWorld.Instance.addSystem<EventDispatchSystem>();
            //
            ECSWorld.Instance.addSystem<DisposeSystem>();
        }

        private void Update()
        {
            ECSWorld.Instance.update();
        }
    }
}
