using UnityEngine;

namespace Mariolike
{
    public class StageScript : MonoBehaviour
    {
        private void Awake()
        {
            ECSWorld.Instance.addSystem<KeyboardControlSystem>();
            ECSWorld.Instance.addSystem<AISimpleMoveSystem>();
            //
            ECSWorld.Instance.addSystem<MoveSystem>();
            ECSWorld.Instance.addSystem<JumpSystem>();
            ECSWorld.Instance.addSystem<GravitySystem>();
            //
            ECSWorld.Instance.addSystem<HitTestGroundSystem>();
            ECSWorld.Instance.addSystem<HitTestWallSystem>();
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
    }
}
