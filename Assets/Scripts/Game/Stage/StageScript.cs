using UnityEngine;

namespace Mariolike
{
    public class StageScript : MonoBehaviour
    {
        public CameraScript cameraScript = null;
        public Transform born = null;

        private void Start()
        {
            GameManager.Instance.registerStage(this);
        }
    }
}
