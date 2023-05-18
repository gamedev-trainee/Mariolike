using UnityEngine;

namespace Mariolike
{
    public class BrickScript : ObjectScript
    {
        public GameObject prop = null;
        public TriggerTimingDefine propBornTiming = TriggerTimingDefine.BeattackEnd;
        public Vector3 propBornOffset = Vector3.zero;
        public int propMaxCount = 0;
        public bool propKillOnStart = false;

        private int m_propGeneratedCount = 0;

        protected override void onBeattackStart(ObjectScript attacker)
        {
            base.onBeattackStart(attacker);

            if (propBornTiming == TriggerTimingDefine.BeattackStart)
            {
                onGenerateProp(attacker);
            }
        }

        protected override void onBeattackEnd(ObjectScript attacker)
        {
            base.onBeattackEnd(attacker);

            if (propBornTiming == TriggerTimingDefine.BeattackEnd)
            {
                onGenerateProp(attacker);
            }
        }

        protected void onGenerateProp(ObjectScript attacker)
        {
            if (prop != null)
            {
                if (propMaxCount > 0)
                {
                    if (m_propGeneratedCount >= propMaxCount)
                    {
                        return;
                    }
                }
                m_propGeneratedCount++;
                GameObject propInst = GameObject.Instantiate(prop);
                propInst.transform.position = transform.position + propBornOffset;
                propInst.transform.localScale = Vector3.one;
                propInst.transform.localRotation = Quaternion.identity;
                if (propKillOnStart)
                {
                    PropScript propScript = propInst.GetComponent<PropScript>();
                    if (propScript != null)
                    {
                        propScript.beattack(attacker);
                    }
                }
            }
        }
    }
}
