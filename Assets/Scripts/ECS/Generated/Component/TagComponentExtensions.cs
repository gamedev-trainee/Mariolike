/////////////////////////////////////////////////
//
// Generated By ECSlikeGenerator
//
/////////////////////////////////////////////////

namespace Mariolike
{
    public static class TagComponentExtensions
    {
        public static ECSlike.IComponent Create(ECSlike.IComponentConfig config)
        {
            TagComponent component = new TagComponent();
            component.setup(config as TagConfigScript);
            return component;
        }

        public static void setup(this TagComponent self, TagConfigScript config)
        {
            self.tags = config.tags;
        }
    }
}