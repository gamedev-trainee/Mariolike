using ECSlike;

namespace Mariolike
{
    public class JumpComponent : IComponent
    {
        public enum JumpStates
        {
            None,
            Jumping,
        }

        public static readonly string JumpStateParameter = "jump_state";

        [ConfigField]
        public float jumpSpeed = 0f;

        // 输入

        public int iNextJumpDir = 0;

        // 存储

        public int mCurJumpDir = 0;
        public float mCurJumpSpeed = 0f;

        public void startJump()
        {
            iNextJumpDir = 1;
        }
    }
}
