using System.Collections.Generic;
using System.ComponentModel;

namespace Mariolike
{
    [DisplayName("collection")]
    public class ActionCollectionClip : ActionClip
    {
        public SortingTypes sortingType = SortingTypes.Order;
        public List<ActionClip> actions = new List<ActionClip>();
    }
}

