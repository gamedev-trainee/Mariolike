using System.Collections.Generic;
using UnityEngine;

namespace Mariolike
{
    public interface IGameEventListener
    {
        void onHostAttrChanged(AttrTypes attr, int value);
    }

    public class GameManager : IECSWorldEventListener
    {
        public static GameManager Instance { get; } = new GameManager();

        private GameStates m_state = GameStates.None;

        private int m_hostEntity = 0;

        private List<IGameEventListener> m_listeners = new List<IGameEventListener>();

        private int m_result = 0;

        public GameManager()
        {
            ECSWorld.Instance.setListener(this);
        }

        public void addListener(IGameEventListener value)
        {
            if (m_listeners.Contains(value)) return;
            m_listeners.Add(value);
        }

        public void removeListener(IGameEventListener value)
        {
            m_listeners.Remove(value);
        }

        public void registerHost(int entity)
        {
            if (m_hostEntity == entity) return;
            m_hostEntity = entity;
            if (m_hostEntity != 0)
            {
                m_result = 0;
                m_state = GameStates.Running;
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

        // IECSWorldEventListener

        public void onHostInit(int entity)
        {
            registerHost(entity);
            CameraScript cameraScript = Camera.main.GetComponent<CameraScript>();
            if (cameraScript != null)
            {
                TransformComponent transformComponent = ECSWorld.Instance.getComponent<TransformComponent>(entity);
                if (transformComponent != null)
                {
                    cameraScript.followTarget = transformComponent.transform;
                }
            }
        }

        public void onHostAttrChanged(AttrTypes type, int value)
        {
            int count = m_listeners.Count;
            for (int i = 0; i < count; i++)
            {
                m_listeners[i].onHostAttrChanged(type, value);
            }
        }

        public void onWorldEvent(EventTypes type)
        {
            handleEvent(type);
        }
    }
}
