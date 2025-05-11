using UnityEngine;
using UnityEngine.EventSystems;

namespace Mariolike
{
    public class UIManagerScript : MonoBehaviour
    {
        public Canvas canvas = null;
        public EventSystem eventSystem = null;

        public string entryUI = string.Empty;
        public string loadingUI = string.Empty;
        public string stageUI = string.Empty;
        public string winUI = string.Empty;
        public string loseUI = string.Empty;

        private void Start()
        {
            UIManager.Instance.setup(this);
        }
    }
}
