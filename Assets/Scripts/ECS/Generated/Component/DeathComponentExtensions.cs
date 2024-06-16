/////////////////////////////////////////////////
//
// Generated By ECSlikeGenerator
//
/////////////////////////////////////////////////

namespace Mariolike
{
    public static class DeathComponentExtensions
    {
        public static ECSlike.IComponent Create(ECSlike.IComponentConfig config)
        {
            DeathComponent component = new DeathComponent();
            component.setup(config as DeathConfigScript);
            return component;
        }

        public static void setup(this DeathComponent self, DeathConfigScript config)
        {
            self.condition = config.condition;
            self.motion = config.motion;
        }
    }
}