using UnityEngine;
using UnityEngine.UI;

namespace Mariolike
{
    public class UIScoreboardScript : MonoBehaviour, IObjectScriptEventListener
    {
        public Text hpValue = null;
        public Text scoreValue = null;

        private ObjectScript m_target = null;

        private void Start()
        {
            if (hpValue != null) hpValue.text = "0";
            if (scoreValue != null) scoreValue.text = "0";
            GameObject go = GameObject.FindGameObjectWithTag(GameObjectTags.Player);
            if (go != null)
            {
                m_target = go.GetComponent<ObjectScript>();
                if (m_target != null)
                {
                    m_target.addListener(this);
                }
            }
        }

        private void OnDestroy()
        {
            if (m_target != null)
            {
                m_target.removeListener(this);
                m_target = null;
            }
        }

        // IObjectScriptEventListener

        public void onObjectStarted()
        {
            if (hpValue != null)
            {
                hpValue.text = (m_target.getAttr(AttrTypes.HP) + m_target.getAttr(AttrTypes.ScaleHP)).ToString();
            }
            if (scoreValue != null)
            {
                scoreValue.text = m_target.getAttr(AttrTypes.Score).ToString();
            }
        }

        public void onObjectAttrChanged(AttrTypes attr, int value)
        {
            switch (attr)
            {
                case AttrTypes.HP:
                    {
                        if (hpValue != null)
                        {
                            hpValue.text = (m_target.getAttr(AttrTypes.HP) + m_target.getAttr(AttrTypes.ScaleHP)).ToString();
                        }
                    }
                    break;
                case AttrTypes.Score:
                    {
                        if (scoreValue != null)
                        {
                            scoreValue.text = value.ToString();
                        }
                    }
                    break;
            }
        }
    }
}
