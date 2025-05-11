using UnityEngine;

namespace Mariolike
{
    public class LoadManagerScript : MonoBehaviour
    {
        private void Update()
        {
            LoadManager.Instance.update();
        }
    }
}
