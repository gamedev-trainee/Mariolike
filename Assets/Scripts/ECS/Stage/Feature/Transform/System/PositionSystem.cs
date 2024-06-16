using ECSlike;

namespace Mariolike
{
    public class PositionSystem : EntitySystem
    {
        protected sealed override System.Type[] getWantedTypes()
        {
            return new System.Type[]
            {
                typeof(PositionComponent),
                typeof(TransformComponent),
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
            PositionComponent positionComponent = world.getComponent<PositionComponent>(entity);
            if (!positionComponent.changed) return;
            if (positionComponent.offset.x != 0f || positionComponent.offset.y != 0f || positionComponent.offset.z != 0f)
            {
                positionComponent.position += positionComponent.offset;
                positionComponent.offset.Set(0f, 0f, 0f);
                TransformComponent transformComponent = world.getComponent<TransformComponent>(entity);
                transformComponent.transform.position = positionComponent.position;
            }
            positionComponent.changed = false;
        }
    }
}
