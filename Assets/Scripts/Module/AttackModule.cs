using UnityEngine;

namespace Mariolike
{
    public class AttackModule
    {
        private int m_instanceID = 0;
        private CharacterGroups m_targetGroup = CharacterGroups.None;
        private Vector3 m_rangeOffset = Vector3.zero;
        private Vector3 m_rangeSize = Vector3.zero;
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

        public void setLayerMask(int value)
        {
            m_layerMask = value;
        }

        public void update(Vector3 pos)
        {
            Collider[] colliders = Physics.OverlapBox(pos + m_rangeOffset, m_rangeSize * 0.5f, Quaternion.identity, m_layerMask);
            if (colliders != null && colliders.Length > 0)
            {
                CharacterScript targetScript;
                int count = colliders.Length;
                for (int i = 0; i < count; i++)
                {
                    targetScript = colliders[i].GetComponent<CharacterScript>();
                    if (targetScript == null) continue;
                    if (targetScript.gameObject.GetInstanceID() == m_instanceID) continue;
                    if (targetScript.group != m_targetGroup) continue;
                    targetScript.kill();
                }
            }
        }
    }
}
