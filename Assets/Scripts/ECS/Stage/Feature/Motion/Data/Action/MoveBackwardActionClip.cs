using System.ComponentModel;

namespace Mariolike
{
    [DisplayName("move_backward")]
    public class MoveBackwardActionClip : TimeActionClip
    {
        public float distance = 0f;
        public bool ignoreCollision = false;
    }
}
