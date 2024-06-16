using ECSlike;
using System.Collections.Generic;

namespace Mariolike
{
    public class TagComponent : IComponent
    {
        [ConfigField]
        public List<Tags> tags = new List<Tags>();

        //

        public bool containsTag(Tags tag)
        {
            return tags.Contains(tag);
        }
    }
}
