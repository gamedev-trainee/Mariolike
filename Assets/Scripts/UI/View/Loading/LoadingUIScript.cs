using UnityEngine;
using UnityEngine.UI;

namespace Mariolike
{
    public class LoadingUIScript : UIViewScript
    {
        public Image mask = null;
        public float animateSpeed = 1f;

        private Color m_color = Color.black;
        private bool m_animating = false;
        private float m_targetAlpha = 0f;
        private System.Action m_callback = null;

        private void Update()
        {
            onUpdateAnimation();
        }

        // 

        protected override void onShow()
        {
            stop();
            turnOn();
        }

        protected override void onHide()
        {
            stop();
            turnOff();
        }

        //

        protected void turnOn()
        {
            m_targetAlpha = 1f;
            m_animating = true;
            m_callback = onTurnOn;
            setAlpha(0f);
            gameObject.SetActive(true);
        }

        protected void turnOff()
        {
            m_targetAlpha = 0f;
            m_animating = true;
            m_callback = onTurnOff;
        }

        protected void onTurnOn()
        {
            onShowCallback();
        }

        protected void onTurnOff()
        {
            gameObject.SetActive(false);
            onHideCallback();
        }

        protected void onUpdateAnimation()
        {
            if (!m_animating) return;
            if (m_color.a < m_targetAlpha)
            {
                m_color.a += animateSpeed * Time.deltaTime;
                if (m_color.a >= m_targetAlpha)
                {
                    m_color.a = m_targetAlpha;
                    m_animating = false;
                }
            }
            else if (m_color.a > m_targetAlpha)
            {
                m_color.a -= animateSpeed * Time.deltaTime;
                if (m_color.a <= m_targetAlpha)
                {
                    m_color.a = m_targetAlpha;
                    m_animating = false;
                }
            }
            else
            {
                m_animating = false;
            }
            mask.color = m_color;
            if (!m_animating)
            {
                if (m_callback != null)
                {
                    System.Action action = m_callback;
                    m_callback = null;
                    action.Invoke();
                }
            }
        }

        protected void stop()
        {
            if (!m_animating) return;
            m_animating = false;
            System.Action callback = m_callback;
            m_callback = null;
            callback?.Invoke();
        }

        protected void setAlpha(float alpha)
        {
            m_color.a = alpha;
            mask.color = m_color;
        }
    }
}
