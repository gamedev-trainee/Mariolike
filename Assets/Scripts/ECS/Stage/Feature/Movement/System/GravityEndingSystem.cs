using ECSlike;

namespace Mariolike
{
    public class GravityEndingSystem : EntitySystem
    {
        protected sealed override System.Type[] getWantedTypes()
        {
            return new System.Type[]
            {
                typeof(GravityComponent),
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
            HitTestGroundComponent hitTestGroundComponent = entity.getComponent<HitTestGroundComponent>();
            if (hitTestGroundComponent.mHitFlags == HitTestFlags.OnGround)
            {
                GravityComponent gravityComponent = entity.getComponent<GravityComponent>();
                gravityComponent.mCurGravity = 0;
                PositionComponent positionComponent = entity.getComponent<PositionComponent>();
                if (positionComponent.offset.y < 0f)
                {
                    positionComponent.offset.y = 0;
                }
            }
        }
    }
}
