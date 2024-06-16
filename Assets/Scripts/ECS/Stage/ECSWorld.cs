using System.Collections.Generic;
using UnityEngine;

namespace Mariolike
{
    public interface IECSWorldUIAgent
    {
        Vector3 getUIAttrWorldPosition(AttrTypes attrType, float z);
    }

    public interface IECSWorldEventListener
    {
        void onHostInit(int entity);
        void onHostAttrChanged(AttrTypes type, int value);
        void onWorldEvent(EventTypes type);
    }

    public class ECSWorld : ECSlike.World
    {
        public static ECSWorld Instance { get; } = new ECSWorld();

        private int m_hostEntity = 0;

        private IECSWorldUIAgent m_uiAgent = null;
        private IECSWorldEventListener m_listener = null;

        public ECSWorld() : base(new ECSWorldInitializer().getRegisterDatas())
        {

        }

        public void setUIAgent(IECSWorldUIAgent value)
        {
            m_uiAgent = value;
        }

        public void setListener(IECSWorldEventListener value)
        {
            m_listener = value;
            if (m_listener != null)
            {
                if (m_hostEntity != 0)
                {
                    onInitHost(m_hostEntity, m_listener);
                }
            }
        }

        public int createEntityBy(EntityScript script)
        {
            int entity = createEntityBy(script.gameObject);
            TransformComponent transformComponent = getComponent<TransformComponent>(entity);
            if (transformComponent == null)
            {
                transformComponent = addComponent<TransformComponent>(entity);
                transformComponent.transform = script.transform;
            }
            PositionComponent positionComponent = addComponent<PositionComponent>(entity);
            positionComponent.position = script.transform.position;
            RotationComponent rotationComponent = addComponent<RotationComponent>(entity);
            rotationComponent.rotation = script.transform.eulerAngles;
            EventListenerComponent eventListenerComponent = addComponent<EventListenerComponent>(entity);
            eventListenerComponent.mCurEventListeners.Add(onReceiveEntityEvent);
            TagComponent tagComponent = getComponent<TagComponent>(entity);
            if (tagComponent != null && tagComponent.containsTag(Tags.Host))
            {
                m_hostEntity = entity;
                if (m_listener != null)
                {
                    onInitHost(m_hostEntity, m_listener);
                }
            }
            return entity;
        }

        public Vector3 getUIAttrPosition(AttrTypes attrType, float z)
        {
            if (m_uiAgent != null) return m_uiAgent.getUIAttrWorldPosition(attrType, z);
            return Vector3.zero;
        }

        protected void onInitHost(int entity, IECSWorldEventListener listener)
        {
            listener.onHostInit(entity);
            AttrComponent attrComponent = getComponent<AttrComponent>(entity);
            if (attrComponent != null)
            {
                foreach (KeyValuePair<AttrTypes, int> kv in attrComponent.mAttrs)
                {
                    listener.onHostAttrChanged(kv.Key, kv.Value);
                }
            }
        }

        protected void onReceiveEntityEvent(EventTypes eventType, int entity, object parameter)
        {
            if (eventType < EventTypes.WorldEventMax)
            {
                m_listener?.onWorldEvent(eventType);
            }
            else
            {
                TagComponent tagComponent = getComponent<TagComponent>(entity);
                if (tagComponent == null || !tagComponent.containsTag(Tags.Host))
                {
                    return;
                }
                switch (eventType)
                {
                    case EventTypes.AttrChange:
                        {
                            AttrTypes attrType = (AttrTypes)parameter;
                            AttrComponent attrComponent = getComponent<AttrComponent>(entity);
                            int value = attrComponent.getAttr(attrType);
                            m_listener?.onHostAttrChanged(attrType, value);
                        }
                        break;
                }
            }
        }
    }
}
