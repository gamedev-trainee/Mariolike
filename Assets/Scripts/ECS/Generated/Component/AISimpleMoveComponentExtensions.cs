/////////////////////////////////////////////////
//
// Generated By ECSlikeGenerator
//
/////////////////////////////////////////////////

namespace Mariolike
{
    public static class AISimpleMoveComponentExtensions
    {
        public static ECSlike.IComponent Create(ECSlike.IComponentConfig config)
        {
            AISimpleMoveComponent component = new AISimpleMoveComponent();
            component.setup(config as AISimpleMoveConfigScript);
            return component;
        }

        public static void setup(this AISimpleMoveComponent self, AISimpleMoveConfigScript config)
        {

        }
    }
}