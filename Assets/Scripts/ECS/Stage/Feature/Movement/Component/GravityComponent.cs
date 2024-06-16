using ECSlike;

namespace Mariolike
{
    public class GravityComponent : IComponent
    {
        [ConfigField]
        public float gravity = 0f;

        public float mCurGravity = 0f;
    }
}
