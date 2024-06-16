using System.ComponentModel;

namespace Mariolike
{
    [DisplayName("play_motion")]
    public class PlayMotionActionClip : ActionClip
    {
        public MotionClip motion = null;
        public bool waitEnd = true;
    }
}
