using ECSlike;

namespace Mariolike
{
    public class MotionPlayData
    {
        public MotionClip motion = null;
        public Entity trigger = Entity.Null;
        public System.Action<object> callback = null;
        public object parameter = null;
    }
}
