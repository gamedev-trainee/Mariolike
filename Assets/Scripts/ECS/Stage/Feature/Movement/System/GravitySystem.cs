using ECSlike;

namespace Mariolike
{
    public class GravitySystem : EntitySystem
    {
        protected sealed override System.Type[] getWantedTypes()
        {
            return new System.Type[]
            {
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
            GravityComponent gravityComponent = entity.getComponent<GravityComponent>();
            if (gravityComponent.mCurGravity <= 0f)
            {
                gravityComponent.mCurGravity = gravityComponent.gravity;
            }
            PositionComponent positionComponent = entity.getComponent<PositionComponent>();
            positionComponent.addY(-gravityComponent.mCurGravity * UnityEngine.Time.deltaTime);
            gravityComponent.mCurGravity += gravityComponent.gravity * UnityEngine.Time.deltaTime;
        }
    }
}
