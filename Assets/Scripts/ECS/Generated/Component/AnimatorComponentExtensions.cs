/////////////////////////////////////////////////
//
// Generated By ECSlikeGenerator
//
/////////////////////////////////////////////////

namespace Mariolike
{
    public static class AnimatorComponentExtensions
    {
        public static ECSlike.IComponent Create(ECSlike.IComponentConfig config)
        {
            AnimatorComponent component = new AnimatorComponent();
            component.setup(config as AnimatorConfigScript);
            return component;
        }

        public static void setup(this AnimatorComponent self, AnimatorConfigScript config)
        {
            self.animator = config.animator;
        }
    }
}