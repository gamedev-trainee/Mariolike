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
                typeof(HitTestComponent),
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
            HitTestComponent hitTestGroundComponent = entity.getComponent<HitTestComponent>();
            if (hitTestGroundComponent.isHitGround())
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
