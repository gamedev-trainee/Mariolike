using UnityEngine;

namespace Mariolike
{
    public class BrickScript : ObjectScript
    {
        public GameObject prop = null;
        public Vector3 propBornOffset = Vector3.zero;
        public int propCount = 0;

        private int m_propGeneratedCount = 0;

        protected override void onBeattackEnd()
        {
            base.onBeattackEnd();

            if (prop != null)
            {
                if (propCount > 0)
                {
                    if (m_propGeneratedCount >= propCount)
                    {
                        return;
                    }
                }
                m_propGeneratedCount++;
                GameObject propInst = GameObject.Instantiate(prop);
                propInst.transform.position = transform.position + propBornOffset;
                propInst.transform.localScale = Vector3.one;
                propInst.transform.localRotation = Quaternion.identity;
            }
        }
    }
}
