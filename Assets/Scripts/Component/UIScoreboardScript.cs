using UnityEngine;
using UnityEngine.UI;

namespace Mariolike
{
    public class UIScoreboardScript : MonoBehaviour, IGameEventListener
    {
        public Text hpValue = null;
        public Text scoreValue = null;

        private void Start()
        {
            if (hpValue != null) hpValue.text = "0";
            if (scoreValue != null) scoreValue.text = "0";
            GameManager.Instance.addListener(this);
        }

        private void OnDestroy()
        {
            GameManager.Instance.removeListener(this);
        }

        // IGameEventListener

        public void onHostAttrChanged(AttrTypes attr, int value)
        {
            switch (attr)
            {
                case AttrTypes.HP:
                    {
                        if (hpValue != null)
                        {
                            hpValue.text = value.ToString();
                        }
                    }
                    break;
                case AttrTypes.Score:
                    {
                        if (scoreValue != null)
                        {
                            scoreValue.text = value.ToString();
                        }
                    }
                    break;
            }
        }
    }
}
