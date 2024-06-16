using UnityEngine;

namespace Mariolike
{
    public class TimeActionPlayer : ActionPlayer
    {
        private float m_passedTime = 0f;

        private TimeActionClip clip => getData<TimeActionClip>();

        public sealed override void play()
        {
            base.play();

            m_passedTime = 0f;
            onPlay();
        }

        protected virtual void onPlay()
        {

        }

        public sealed override void update()
        {
            base.update();

            m_passedTime += Time.deltaTime;
            onUpdate();
            if (m_passedTime >= clip.duration)
            {
                setDone();
            }
        }

        protected virtual void onUpdate()
        {

        }

        protected float getProgress()
        {
            return clip.curve.Evaluate(Mathf.Clamp01(m_passedTime / clip.duration));
        }
    }
}
