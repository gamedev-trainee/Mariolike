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

        protected override void onUpdateEntity(World world, int entity)
        {
            HitTestGroundComponent hitTestGroundComponent = world.getComponent<HitTestGroundComponent>(entity);
            if (hitTestGroundComponent.mHitFlags == HitTestFlags.OnGround)
            {
                GravityComponent gravityComponent = world.getComponent<GravityComponent>(entity);
                gravityComponent.mCurGravity = 0;
                PositionComponent positionComponent = world.getComponent<PositionComponent>(entity);
                if (positionComponent.offset.y < 0f)
                {
                    positionComponent.offset.y = 0;
                }
            }
        }
    }
}
