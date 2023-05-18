using UnityEngine;

namespace Mariolike
{
    public class AttackModule
    {
        private int m_instanceID = 0;
        private CharacterGroups m_targetGroup = CharacterGroups.None;
        private Vector3 m_rangeOffset = Vector3.zero;
        private Vector3 m_rangeSize = Vector3.zero;
        private float m_rangeScale = 1f;
        private int m_layerMask = 0;

        public void setInstanceID(int value)
        {
            m_instanceID = value;
        }

        public void setTargetGroup(CharacterGroups value)
        {
            m_targetGroup = value;
        }

        public void setRangeOffset(Vector3 value)
        {
            m_rangeOffset = value;
        }

        public void setRangeSize(Vector3 value)
        {
            m_rangeSize = value;
        }

        public void setRangeScale(float value)
        {
            m_rangeScale = value;
        }

        public void setLayerMask(int value)
        {
            m_layerMask = value;
        }

        public bool update(Vector3 pos, ObjectScript attacker)
        {
            Collider[] colliders = Physics.OverlapBox(pos + m_rangeOffset * m_rangeScale, m_rangeSize * m_rangeScale * 0.5f, Quaternion.identity, m_layerMask);
            if (colliders != null && colliders.Length > 0)
            {
                bool attacked = false;
                ObjectScript targetScript;
                int count = colliders.Length;
                for (int i = 0; i < count; i++)
                {
                    targetScript = colliders[i].GetComponent<ObjectScript>();
                    if (targetScript == null) continue;
                    if (targetScript.gameObject.GetInstanceID() == m_instanceID) continue;
                    if (m_targetGroup != CharacterGroups.None)
                    {
                        if (!(targetScript is CharacterScript)) continue;
                        if ((targetScript as CharacterScript).group != m_targetGroup) continue;
                    }
                    else
                    {
                        if (targetScript is CharacterScript) continue;
                    }
                    targetScript.beattack(attacker);
                    attacked = true;
                }
                return attacked;
            }
            return false;
        }
    }
}
