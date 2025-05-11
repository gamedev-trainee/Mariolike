using UnityEngine;

namespace Mariolike
{
    public class ResourcesLoader<T> : ILoader<T> where T : Object
    {
        private string m_address = string.Empty;
        private System.Action<T> m_callback = null;
        private T m_asset = null;
        private bool m_isDone = false;

        private ResourceRequest m_request = null;

        public void addCallback(System.Action<T> callback)
        {
            if (callback == null) return;
            m_callback += callback;
        }

        public T getAsset()
        {
            return m_asset;
        }

        public void setAddress(string value)
        {
            m_address = value;
        }

        public void load()
        {
            if (m_request != null) return;
            m_request = Resources.LoadAsync<T>(m_address);
        }

        public void update()
        {
            if (m_request.isDone)
            {
                m_asset = m_request.asset as T;
                m_isDone = true;
            }
        }

        public bool isDone()
        {
            return m_isDone;
        }

        public void invoke()
        {
            if (m_callback != null)
            {
                m_callback.Invoke((T)m_request.asset);
                m_callback = null;
            }
        }
    }
}
