using UnityEngine;

namespace Mariolike
{
    public class GameManagerScript : MonoBehaviour
    {
        public WorldSettings world = null;
        public string playerResource = string.Empty;
        public string deathHoleResource = string.Empty;
        public string stageResourceFormat = string.Empty;
        public int maxStage = 0;

        private void Start()
        {
            GameManager.Instance.setup(this);
        }

        private void Update()
        {
            GameManager.Instance.update();
        }
    }
}
