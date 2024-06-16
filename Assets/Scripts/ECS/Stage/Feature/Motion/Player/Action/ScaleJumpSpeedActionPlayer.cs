namespace Mariolike
{
    public class ScaleJumpSpeedActionPlayer : ActionPlayer
    {
        private ScaleJumpSpeedActionClip clip => getData<ScaleJumpSpeedActionClip>();

        public override void play()
        {
            base.play();

            JumpComponent jumpComponent = getComponent<JumpComponent>();
            if (jumpComponent != null)
            {
                jumpComponent.mCurJumpSpeed *= clip.scale;
            }
            setDone();
        }

        protected void onMotionComplete(object parameter)
        {
            setDone();
        }
    }
}
