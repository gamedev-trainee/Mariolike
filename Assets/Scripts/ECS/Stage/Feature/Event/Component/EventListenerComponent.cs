using ECSlike;
using System.Collections.Generic;

namespace Mariolike
{
    public delegate void EventCallback(EventTypes type, Entity entity, object parameter);

    public class EventListenerComponent : IComponent
    {
        //

        //

        public List<EventCallback> mCurEventListeners = new List<EventCallback>();
    }
}
