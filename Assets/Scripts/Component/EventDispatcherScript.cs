
namespace Mariolike
{
    public class EventDispatcherScript : ObjectScript
    {
        public TriggerTimingDefine dispatchTiming = TriggerTimingDefine.BeattackEnd;
        public EventTypes dispatchEvt = EventTypes.Unknown;
        public int dispatchMaxCount = 0;

        private int m_evtDispatchedCount = 0;

        protected override void onBeattackStart(ObjectScript attacker)
        {
            base.onBeattackStart(attacker);

            if (dispatchTiming == TriggerTimingDefine.BeattackStart)
            {
                onDispatchEvent();
            }
        }

        protected override void onBeattackEnd(ObjectScript attacker)
        {
            base.onBeattackEnd(attacker);

            if (dispatchTiming == TriggerTimingDefine.BeattackEnd)
            {
                onDispatchEvent();
            }
        }

        protected void onDispatchEvent()
        {
            if (dispatchEvt != EventTypes.Unknown)
            {
                if (dispatchMaxCount > 0)
                {
                    if (m_evtDispatchedCount >= dispatchMaxCount)
                    {
                        return;
                    }
                }
                GameManager.Instance.handleEvent(dispatchEvt);
            }
        }
    }
}
