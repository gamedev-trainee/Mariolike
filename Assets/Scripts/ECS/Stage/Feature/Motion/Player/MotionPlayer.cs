﻿using ECSlike;

namespace Mariolike
{
    public class MotionPlayer
    {
        private Entity m_entity = Entity.Null;
        private Entity m_trigger = Entity.Null;
        private MotionClip m_data = null;

        private bool m_isDone = false;

        private ActionPlayer m_curPlayer = null;

        public void setEntity(Entity value)
        {
            m_entity = value;
        }

        public void setTrigger(Entity value)
        {
            m_trigger = value;
        }

        public void setData(MotionClip value)
        {
            m_data = value;
        }

        public void play()
        {
            m_curPlayer = MotionUtils.CreateActionPlayer(m_data.action.GetType());
            m_curPlayer.setEntity(m_entity);
            m_curPlayer.setTrigger(m_trigger);
            m_curPlayer.setData(m_data.action);
            m_curPlayer.play();
        }

        public void update()
        {
            if (m_curPlayer.isDone())
            {
                m_isDone = true;
            }
            else
            {
                m_curPlayer.update();
            }
        }

        public bool isDone()
        {
            return m_isDone;
        }

        public void dispose()
        {
            if (m_curPlayer != null)
            {
                m_curPlayer.dispose();
                m_curPlayer = null;
            }
            m_data = null;
        }
    }
}
