using UnityEngine;

namespace Mariolike
{
    public class MoveBackwardActionPlayer : TimeActionPlayer
    {
        private PositionComponent m_positionComponent = null;
        private Vector3 m_startPosition = Vector3.zero;
        private int m_forward = 0;

        private MoveBackwardActionClip clip => getData<MoveBackwardActionClip>();

        protected override void onPlay()
        {
            base.onPlay();

            m_positionComponent = getComponent<PositionComponent>();
            if (m_positionComponent != null)
            {
                m_startPosition = m_positionComponent.position;
                RotationComponent rotationComponent = getComponent<RotationComponent>();
                m_forward = rotationComponent.rotation.y == 180 ? -1 : 1;
            }
            else
            {
                setDone();
            }
        }

        protected override void onUpdate()
        {
            base.onUpdate();

            Vector3 position = m_positionComponent.position;
            float movedDistance = clip.distance * getProgress();
            position.x = m_startPosition.x - movedDistance * m_forward;
            if (clip.ignoreCollision)
            {
                m_positionComponent.position = position;
            }
            else
            {
                m_positionComponent.offset.x += position.x - m_positionComponent.position.x;
            }
            m_positionComponent.changed = true;
        }
    }
}
