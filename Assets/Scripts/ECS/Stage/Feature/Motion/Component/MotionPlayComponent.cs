using ECSlike;
using System.Collections.Generic;

namespace Mariolike
{
    public class MotionPlayComponent : IComponent
    {
        //

        public List<MotionPlayData> iNextMotions = new List<MotionPlayData>();

        //

        public MotionPlayData iCurMotionData = null;
        public MotionPlayer iCurMotionPlayer = null;

        //

        public void playMotion(MotionClip motion, Entity trigger)
        {
            iNextMotions.Add(new MotionPlayData()
            {
                motion = motion,
                trigger = trigger,
            });
        }

        public void playMotion(MotionClip motion, Entity trigger, System.Action<object> callback, object parameter = null)
        {
            iNextMotions.Add(new MotionPlayData()
            {
                motion = motion,
                trigger = trigger,
                callback = callback,
                parameter = parameter,
            });
        }
    }
}
