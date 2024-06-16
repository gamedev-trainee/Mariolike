using ECSlike;

namespace Mariolike
{
    public partial class GroupComponent : IComponent
    {
        [ConfigField]
        public FightGroups group = FightGroups.None;
    }
}
