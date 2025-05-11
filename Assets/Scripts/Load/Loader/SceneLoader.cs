using UnityEngine;
using UnityEngine.SceneManagement;

namespace Mariolike
{
    public class SceneLoader : ISceneLoader
    {
        private string m_address = string.Empty;
        private System.Action<Scene> m_callback = null;
        private bool m_isDone = false;

        private AsyncOperation m_request = null;

        public void addCallback(System.Action<Scene> callback)
        {
            if (callback == null) return;
            m_callback += callback;
        }

        public Scene getScene()
        {
            return SceneManager.GetSceneByName(m_address);
        }

        public void setAddress(string value)
        {
            m_address = value;
        }

        public void load()
        {
            if (m_request != null) return;
            m_request = SceneManager.LoadSceneAsync(m_address);
        }

        public void update()
        {
            if (m_request.isDone)
            {
                m_isDone = true;
            }
        }

        public bool isDone()
        {
            return m_isDone;
        }

        public void invoke()
        {
            m_callback?.Invoke(SceneManager.GetSceneByName(m_address));
        }

        public void reset()
        {
            m_isDone = false;
            m_request = null;
        }
    }
}
