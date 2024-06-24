using ECSlike;

namespace Mariolike
{
    public class ActionPlayer
    {
        private Entity m_entity = Entity.Null;
        private Entity m_trigger = Entity.Null;
        private ActionClip m_data = null;

        private bool m_isDone = false;

        public void setEntity(Entity value)
        {
            m_entity = value;
        }

        protected Entity getEntity()
        {
            return m_entity;
        }

        public void setTrigger(Entity value)
        {
            m_trigger = value;
        }

        protected Entity getTrigger()
        {
            return m_trigger;
        }

        protected T getComponent<T>() where T : IComponent
        {
            if (m_entity.isNull()) return default(T);
            return m_entity.getComponent<T>();
        }

        protected T getOrAddComponent<T>() where T : IComponent, new()
        {
            if (m_entity.isNull()) return default(T);
            return m_entity.getOrAddComponent<T>();
        }

        public void setData(ActionClip value)
        {
            m_data = value;
        }

        protected T getData<T>() where T : ActionClip
        {
            return (T)m_data;
        }

        public virtual void play()
        {

        }

        public virtual void update()
        {

        }

        public bool isDone()
        {
            return m_isDone;
        }

        protected void setDone()
        {
            m_isDone = true;
        }

        public virtual void dispose()
        {
            m_data = null;
        }
    }
}
