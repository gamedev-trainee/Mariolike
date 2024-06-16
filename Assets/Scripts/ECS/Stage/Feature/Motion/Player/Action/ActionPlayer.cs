using ECSlike;

namespace Mariolike
{
    public class ActionPlayer
    {
        private World m_world = null;
        private int m_entity = 0;
        private int m_trigger = 0;
        private ActionClip m_data = null;

        private bool m_isDone = false;

        public void setWorld(World world)
        {
            m_world = world;
        }

        protected World getWorld()
        {
            return m_world;
        }

        public void setEntity(int value)
        {
            m_entity = value;
        }

        protected int getEntity()
        {
            return m_entity;
        }

        public void setTrigger(int value)
        {
            m_trigger = value;
        }

        protected int getTrigger()
        {
            return m_trigger;
        }

        protected T getComponent<T>() where T : IComponent
        {
            if (m_world == null || m_entity == 0) return default(T);
            return m_world.getComponent<T>(m_entity);
        }

        protected T getOrAddComponent<T>() where T : IComponent, new()
        {
            if (m_world == null || m_entity == 0) return default(T);
            return m_world.getOrAddComponent<T>(m_entity);
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
            m_entity = 0;
        }
    }
}
