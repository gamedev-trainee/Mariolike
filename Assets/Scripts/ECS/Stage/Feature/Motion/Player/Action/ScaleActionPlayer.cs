using UnityEngine;

namespace Mariolike
{
    public class ScaleActionPlayer : TimeActionPlayer
    {
        private Transform m_transform = null;
        private Vector3 m_startScale = Vector3.zero;

        private ScaleActionClip clip => getData<ScaleActionClip>();

        protected override void onPlay()
        {
            base.onPlay();

            TransformComponent transformComponent = getComponent<TransformComponent>();
            m_transform = transformComponent?.transform;
            if (m_transform != null)
            {
                m_startScale = m_transform.localScale;
            }
            else
            {
                setDone();
            }
        }

        protected override void onUpdate()
        {
            base.onUpdate();

            m_transform.localScale = Vector3.Lerp(m_startScale, clip.scale, getProgress());
        }
    }
}
