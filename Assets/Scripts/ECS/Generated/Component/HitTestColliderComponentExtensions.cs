/////////////////////////////////////////////////
//
// Generated By ECSlikeGenerator
//
/////////////////////////////////////////////////

namespace Mariolike
{
    public static class HitTestColliderComponentExtensions
    {
        public static ECSlike.IComponent Create(ECSlike.IComponentConfig config)
        {
            HitTestColliderComponent component = new HitTestColliderComponent();
            component.setup(config as HitTestColliderConfigScript);
            return component;
        }

        public static void setup(this HitTestColliderComponent self, HitTestColliderConfigScript config)
        {
            self.collider = config.GetComponent<UnityEngine.Collider>();
            self.initComponent();
        }
    }
}