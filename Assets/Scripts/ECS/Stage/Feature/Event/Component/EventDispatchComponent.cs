using ECSlike;
using System.Collections.Generic;

namespace Mariolike
{
    public class EventDispatchComponent : IComponent
    {
        //

        public List<EventDispatchData> iNextDispatchEvents = new List<EventDispatchData>();

        public void sendEvent(EventTypes type, object parameter = null)
        {
            iNextDispatchEvents.Add(new EventDispatchData()
            {
                type = type,
                parameter = parameter,
            });
        }
    }
}
