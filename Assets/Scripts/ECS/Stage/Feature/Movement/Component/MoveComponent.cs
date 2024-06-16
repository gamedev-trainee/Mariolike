using ECSlike;

namespace Mariolike
{
    public class MoveComponent : IComponent
    {
        public enum MoveStates
        {
            None,
            Moving,
        }

        public static readonly string MoveStateParameter = "move_state";

        // 属性

        [ConfigField]
        public float moveSpeed = 0f;

        // 输入

        public int iNextMoveDir = 0; // 目标移动方向

        // 存储

        public int mCurMoveDir = 0; // 当前移动方向

        //

        public void startMove(int dir)
        {
            iNextMoveDir = dir;
        }

        public void startMoveLeft()
        {
            iNextMoveDir = -1;
        }

        public void startMoveRight()
        {
            iNextMoveDir = 1;
        }

        public void stopMove()
        {
            iNextMoveDir = 0;
        }

        public void turnAround()
        {
            iNextMoveDir = -mCurMoveDir;
        }

        public bool isMoving()
        {
            return mCurMoveDir != 0;
        }
    }
}
