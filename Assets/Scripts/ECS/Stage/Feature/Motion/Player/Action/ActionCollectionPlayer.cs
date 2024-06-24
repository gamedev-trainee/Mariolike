using System.Collections.Generic;

namespace Mariolike
{
    public class ActionCollectionPlayer : ActionPlayer
    {
        private int m_curIndex = -1;
        private List<ActionPlayer> m_curPlayers = new List<ActionPlayer>();

        private ActionCollectionClip clip => getData<ActionCollectionClip>();

        public override void play()
        {
            base.play();

            m_curIndex = 0;
            if (clip.actions.Count > 0)
            {
                switch (clip.sortingType)
                {
                    case SortingTypes.Order:
                        {
                            ActionPlayer player = createActionPlayer(clip.actions[m_curIndex]);
                            m_curPlayers.Add(player);
                            m_curIndex++;
                        }
                        break;
                    case SortingTypes.Abreast:
                        {
                            ActionPlayer player;
                            int count = clip.actions.Count;
                            for (int i = 0; i < count; i++)
                            {
                                player = createActionPlayer(clip.actions[i]);
                                m_curPlayers.Add(player);
                            }
                            m_curIndex = clip.actions.Count;
                        }
                        break;
                }
                if (m_curPlayers.Count > 0)
                {
                    int count = m_curPlayers.Count;
                    for (int i = 0; i < count; i++)
                    {
                        m_curPlayers[i].play();
                    }
                }
            }
        }

        public override void update()
        {
            base.update();

            if (m_curPlayers.Count > 0)
            {
                int count = m_curPlayers.Count;
                for (int i = 0; i < count; i++)
                {
                    m_curPlayers[i].update();
                    if (m_curPlayers[i].isDone())
                    {
                        m_curPlayers.RemoveAt(i);
                        i--;
                        count--;
                    }
                }
            }
            else
            {
                if (m_curIndex >= clip.actions.Count)
                {
                    setDone();
                }
                else
                {
                    ActionPlayer player = createActionPlayer(clip.actions[m_curIndex]);
                    m_curPlayers.Add(player);
                    m_curIndex++;
                    player.play();
                }
            }
        }

        public override void dispose()
        {
            if (m_curPlayers.Count > 0)
            {
                int count = m_curPlayers.Count;
                for (int i = 0; i < count; i++)
                {
                    m_curPlayers[i].dispose();
                }
                m_curPlayers.Clear();
            }
            base.dispose();
        }

        //

        protected ActionPlayer createActionPlayer(ActionClip action)
        {
            ActionPlayer player = MotionUtils.CreateActionPlayer(action.GetType());
            player.setEntity(getEntity());
            player.setTrigger(getTrigger());
            player.setData(action);
            return player;
        }
    }
}
