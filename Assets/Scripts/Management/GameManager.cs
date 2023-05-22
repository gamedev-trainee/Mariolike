using System.Collections.Generic;

namespace Mariolike
{
    public interface IGameEventListener
    {
        void onHostInited(ObjectScript host);
        void onHostAttrChanged(ObjectScript host, AttrTypes attr, int value);
    }

    public class GameManager : IObjectScriptEventListener
    {
        public static GameManager Instance { get; } = new GameManager();

        private GameStates m_state = GameStates.None;

        private HeroScript m_host = null;

        private List<IGameEventListener> m_listeners = new List<IGameEventListener>();

        private int m_result = 0;

        public void addListener(IGameEventListener value)
        {
            if (m_listeners.Contains(value)) return;
            m_listeners.Add(value);
        }

        public void removeListener(IGameEventListener value)
        {
            m_listeners.Remove(value);
        }

        public void registerHost(HeroScript host)
        {
            if (m_host == host) return;
            m_host = host;
            if (m_host != null)
            {
                m_result = 0;
                m_state = GameStates.Running;
                m_host.addListener(this);
                int count = m_listeners.Count;
                for (int i = 0; i < count; i++)
                {
                    m_listeners[i].onHostInited(m_host);
                }
            }
        }

        public GameStates getState()
        {
            return m_state;
        }

        public void handleEvent(EventTypes evt)
        {
            switch (evt)
            {
                case EventTypes.StageClear:
                    {
                        if (m_state != GameStates.Running) return;
                        m_result = 1;
                        m_state = GameStates.End;
                        UnityEngine.Debug.Log("stage clear");
                    }
                    break;
                case EventTypes.StageFail:
                    {
                        if (m_state != GameStates.Running) return;
                        m_result = -1;
                        m_state = GameStates.End;
                        UnityEngine.Debug.Log("stage fail");
                    }
                    break;
            }
        }

        // IObjectScriptEventListener

        public void onObjectAttrChanged(AttrTypes attr, int value)
        {
            int count = m_listeners.Count;
            for (int i = 0; i < count; i++)
            {
                m_listeners[i].onHostAttrChanged(m_host, attr, value);
            }
        }

        public void onObjectDead()
        {
            handleEvent(EventTypes.StageFail);
        }
    }
}
