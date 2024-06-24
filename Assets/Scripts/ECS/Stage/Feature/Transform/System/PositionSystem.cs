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

        protected override void onUpdateEntity(Entity entity)
        {
            PositionComponent positionComponent = entity.getComponent<PositionComponent>();
            if (!positionComponent.changed) return;
            if (positionComponent.offset.x != 0f || positionComponent.offset.y != 0f || positionComponent.offset.z != 0f)
            {
                positionComponent.position += positionComponent.offset;
                positionComponent.offset.Set(0f, 0f, 0f);
                TransformComponent transformComponent = entity.getComponent<TransformComponent>();
                transformComponent.transform.position = positionComponent.position;
            }
            positionComponent.changed = false;
        }
    }
}
