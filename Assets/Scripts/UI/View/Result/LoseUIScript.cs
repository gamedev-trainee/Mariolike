using UnityEngine;

namespace Mariolike
{
    public class LoseUIScript : UIViewScript
    {
        public float stayTime = 3f;

        private bool m_animating = false;
        private float m_passedTime = 0f;

        private void Update()
        {
            if (m_animating)
            {
                m_passedTime += Time.deltaTime;
                if (m_passedTime >= stayTime)
                {
                    m_animating = false;
                    GameManager.Instance.exitGame();
                }
            }
        }

        protected override void onShow()
        {
            base.onShow();

            m_passedTime = 0f;
            m_animating = true;
        }

        protected override void onHide()
        {
            base.onHide();

            m_animating = false;
        }
    }
}
