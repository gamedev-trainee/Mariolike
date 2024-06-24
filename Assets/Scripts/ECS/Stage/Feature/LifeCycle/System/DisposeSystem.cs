using ECSlike;
using UnityEngine;

namespace Mariolike
{
    public class DisposeSystem : EntitySystem
    {
        protected sealed override System.Type[] getWantedTypes()
        {
            return new System.Type[]
            {
                typeof(DisposeComponent),
            };
        }

        protected override System.Type[] getUnwantedTypes()
        {
            return null;
        }

        protected override void onUpdateEntity(Entity entity)
        {
            TransformComponent transformComponent = entity.getComponent<TransformComponent>();
            if (transformComponent != null)
            {
                GameObject.Destroy(transformComponent.transform.gameObject);
            }
            entity.destroy();
        }
    }
}
