using UnityEngine;
using UnityEngine.UI;

namespace Mariolike
{
    public class StageUIScript : MonoBehaviour, IGameEventListener
    {
        public Text stageValue = null;
        public Text timeValue = null;
        public Text lifeValue = null;
        public Text scoreValue = null;

        private void Start()
        {
            GameManager.Instance.addListener(this);
        }

        private void OnDestroy()
        {
            GameManager.Instance.removeListener(this);
        }

        //

        protected string formatTime(float time)
        {
            int m = Mathf.FloorToInt(time / 60f);
            string ms = m.ToString();
            if (ms.Length < 2) ms = string.Format("0{0}", ms);
            int s = Mathf.FloorToInt(time - m * 60f);
            string ss = s.ToString();
            if (ss.Length < 2) ss = string.Format("0{0}", ss);
            return string.Format("{0}:{1}", ms, ss);
        }

        // IGameEventListener

        public void onStageStart(int stage)
        {
            if (stageValue != null)
            {
                stageValue.text = string.Format("Stage: {0}", stage);
            }
        }

        public void onStageUpdate(float time)
        {
            if (timeValue != null)
            {
                timeValue.text = formatTime(time);
            }
        }

        public void onHostLifeChanged(int value)
        {
            if (lifeValue != null)
            {
                lifeValue.text = value.ToString();
            }
        }

        public void onHostScoreChanged(int value)
        {
            if (scoreValue != null)
            {
                scoreValue.text = value.ToString();
            }
        }
    }
}
