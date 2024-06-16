namespace Mariolike
{
    public class PlayMotionActionPlayer : ActionPlayer
    {
        private PlayMotionActionClip clip => getData<PlayMotionActionClip>();

        public override void play()
        {
            base.play();

            if (clip.motion != null)
            {
                System.Action<object> callback = null;
                if (clip.waitEnd) callback = onMotionComplete;
                MotionPlayComponent motionPlayComponent = getOrAddComponent<MotionPlayComponent>();
                motionPlayComponent.playMotion(clip.motion, getTrigger(), callback);
                if (clip.waitEnd)
                {
                    return;
                }
            }
            setDone();
        }

        protected void onMotionComplete(object parameter)
        {
            setDone();
        }
    }
}
