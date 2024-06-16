using UnityEngine;

namespace Mariolike
{
    public class GameCycle : MonoBehaviour
    {
        private void Update()
        {
            ECSWorld.Instance.update();
        }
    }
}
