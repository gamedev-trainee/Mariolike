/////////////////////////////////////////////////
//
// Generated By ECSlikeGenerator
//
/////////////////////////////////////////////////

namespace Mariolike
{
    public static class AttrComponentExtensions
    {
        public static ECSlike.IComponent Create(ECSlike.IComponentConfig config)
        {
            AttrComponent component = new AttrComponent();
            component.setup(config as AttrConfigScript);
            return component;
        }

        public static void setup(this AttrComponent self, AttrConfigScript config)
        {
            self.attrs = config.attrs;
            self.initComponent();
        }
    }
}