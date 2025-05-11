using ECSlike;
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
        void onHostInit(Entity entity);
        void onHostAttrChanged(AttrTypes type, int value);
        void onHostDead();
        void onWorldEvent(EventTypes type);
    }

    public class ECSWorld : ECSlike.World
    {
        public static ECSWorld Instance { get; } = new ECSWorld();

        private Entity m_hostEntity = Entity.Null;

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
                if (!m_hostEntity.isNull())
                {
                    onInitHost(m_hostEntity, m_listener);
                }
            }
        }

        public Entity createEntityBy(EntityScript script)
        {
            Entity entity = createEntityBy(script.gameObject);
            TransformComponent transformComponent = entity.getComponent<TransformComponent>();
            if (transformComponent == null)
            {
                transformComponent = entity.addComponent<TransformComponent>();
                transformComponent.transform = script.transform;
            }
            PositionComponent positionComponent = entity.addComponent<PositionComponent>();
            positionComponent.position = script.transform.position;
            RotationComponent rotationComponent = entity.addComponent<RotationComponent>();
            rotationComponent.rotation = script.transform.eulerAngles;
            EventListenerComponent eventListenerComponent = entity.addComponent<EventListenerComponent>();
            eventListenerComponent.mCurEventListeners.Add(onReceiveEntityEvent);
            TagComponent tagComponent = entity.getComponent<TagComponent>();
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

        public void destroyEntity(Entity entity)
        {
            if (entity.isNull()) return;
            entity.destroy();
        }

        public Vector3 getUIAttrPosition(AttrTypes attrType, float z)
        {
            if (m_uiAgent != null) return m_uiAgent.getUIAttrWorldPosition(attrType, z);
            return Vector3.zero;
        }

        protected void onInitHost(Entity entity, IECSWorldEventListener listener)
        {
            listener.onHostInit(entity);
            AttrComponent attrComponent = entity.getComponent<AttrComponent>();
            if (attrComponent != null)
            {
                foreach (KeyValuePair<AttrTypes, int> kv in attrComponent.mAttrs)
                {
                    listener.onHostAttrChanged(kv.Key, kv.Value);
                }
            }
        }

        protected void onReceiveEntityEvent(EventTypes eventType, Entity entity, object parameter)
        {
            if (eventType < EventTypes.WorldEventMax)
            {
                m_listener?.onWorldEvent(eventType);
            }
            else
            {
                TagComponent tagComponent = entity.getComponent<TagComponent>();
                if (tagComponent == null || !tagComponent.containsTag(Tags.Host))
                {
                    return;
                }
                switch (eventType)
                {
                    case EventTypes.AttrChange:
                        {
                            AttrTypes attrType = (AttrTypes)parameter;
                            AttrComponent attrComponent = entity.getComponent<AttrComponent>();
                            int value = attrComponent.getAttr(attrType);
                            m_listener?.onHostAttrChanged(attrType, value);
                        }
                        break;
                    case EventTypes.Dead:
                        {
                            m_listener?.onHostDead();
                        }
                        break;
                }
            }
        }
    }
}
