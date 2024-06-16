using System.Collections.Generic;
using System.ComponentModel;

namespace Mariolike
{
    [DisplayName("event_dispatch")]
    public class EventDispatchActionClip : ActionClip
    {
        public List<EventInfo> events = new List<EventInfo>();
    }
}
