using ECSlike;
using UnityEngine;

namespace Mariolike
{
    public class EntityScript : MonoBehaviour
    {
        [HideInInspector]
        public Entity entity = Entity.Null;

        private void Start()
        {
            if (entity.isNull())
            {
                entity = ECSWorld.Instance.createEntityBy(this);
            }
        }

        private void OnDestroy()
        {
            ECSWorld.Instance.destroyEntity(entity);
        }
    }
}
