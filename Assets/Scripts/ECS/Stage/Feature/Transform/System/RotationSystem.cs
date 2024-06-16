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

        protected override void onUpdateEntity(World world, int entity)
        {
            RotationComponent rotationComponent = world.getComponent<RotationComponent>(entity);
            if (!rotationComponent.changed) return;
            TransformComponent transformComponent = world.getComponent<TransformComponent>(entity);
            transformComponent.transform.eulerAngles = rotationComponent.rotation;
            rotationComponent.changed = false;
        }
    }
}
