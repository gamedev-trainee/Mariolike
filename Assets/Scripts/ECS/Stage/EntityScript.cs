using UnityEngine;

namespace Mariolike
{
    public class EntityScript : MonoBehaviour
    {
        [HideInInspector]
        public int entity = 0;

        private void Start()
        {
            if (entity == 0)
            {
                entity = ECSWorld.Instance.createEntityBy(this);
            }
        }
    }
}
