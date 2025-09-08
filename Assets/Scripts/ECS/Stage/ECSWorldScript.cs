using UnityEngine;

namespace Mariolike
{
    public class ECSWorldScript : MonoBehaviour
    {
        private void Update()
        {
            ECSWorld.Instance.update();
        }
    }
}
