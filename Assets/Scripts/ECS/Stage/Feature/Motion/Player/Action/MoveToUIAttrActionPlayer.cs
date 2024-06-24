using UnityEngine;

namespace Mariolike
{
    public class MoveToUIAttrActionPlayer : TimeActionPlayer
    {
        private Transform m_transform = null;
        private Vector3 m_startPosition = Vector3.zero;
        private Vector3 m_endPosition = Vector3.zero;

        private MoveToUIAttrActionClip clip => getData<MoveToUIAttrActionClip>();

        protected override void onPlay()
        {
            base.onPlay();

            TransformComponent transformComponent = getComponent<TransformComponent>();
            m_transform = transformComponent?.transform;
            if (m_transform != null)
            {
                m_startPosition = m_transform.position;
            }
            else
            {
                setDone();
            }
        }

        protected override void onUpdate()
        {
            base.onUpdate();

            m_endPosition = (getEntity().world as ECSWorld).getUIAttrPosition(clip.attrType, m_startPosition.z);
            Vector3 position = Vector3.Lerp(m_startPosition, m_endPosition, getProgress());
            m_transform.position = position;
        }
    }
}
