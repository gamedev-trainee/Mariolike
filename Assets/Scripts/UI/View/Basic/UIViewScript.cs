using System;
using UnityEngine;

namespace Mariolike
{
    public class UIViewScript : MonoBehaviour, IUIViewScript
    {
        private int m_visible = 0;
        private System.Action m_showCallback = null;
        private System.Action m_hideCallback = null;

        //

        protected virtual void onShow()
        {
            gameObject.SetActive(true);
            onShowCallback();
        }

        protected void onShowCallback()
        {
            if (m_showCallback != null)
            {
                System.Action callback = m_showCallback;
                m_showCallback = null;
                callback?.Invoke();
            }
        }

        protected virtual void onHide()
        {
            gameObject.SetActive(false);
            onHideCallback();
        }

        protected void onHideCallback()
        {
            if (m_hideCallback != null)
            {
                System.Action callback = m_hideCallback;
                m_hideCallback = null;
                callback?.Invoke();
            }
        }

        // IUIViewScript

        public void show(Action callback = null)
        {
            if (m_visible > 0)
            {
                callback?.Invoke();
                return;
            }
            m_visible = 1;
            m_showCallback = callback;
            onShow();
        }

        public void hide(Action callback = null)
        {
            if (m_visible < 0)
            {
                callback?.Invoke();
                return;
            }
            m_visible = -1;
            m_hideCallback = callback;
            onHide();
        }
    }
}
