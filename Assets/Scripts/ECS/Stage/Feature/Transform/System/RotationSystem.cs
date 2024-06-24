using ECSlike;

namespace Mariolike
{
    public class RotationSystem : EntitySystem
    {
        protected sealed override System.Type[] getWantedTypes()
        {
            return new System.Type[]
            {
                typeof(RotationComponent),
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
            RotationComponent rotationComponent = entity.getComponent<RotationComponent>();
            if (!rotationComponent.changed) return;
            TransformComponent transformComponent = entity.getComponent<TransformComponent>();
            transformComponent.transform.eulerAngles = rotationComponent.rotation;
            rotationComponent.changed = false;
        }
    }
}
