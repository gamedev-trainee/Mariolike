using ECSlike;

namespace Mariolike
{
    public class FightUtils
    {
        public static bool IsAttackableGroup(FightConnections attackConnection, FightGroups attackGroup, GroupComponent beattackGroup)
        {
            if (attackConnection == FightConnections.All) return true;
            if (beattackGroup != null) return IsAttackableGroup(attackConnection, attackGroup, beattackGroup.group);
            return false;
        }

        public static bool IsAttackableGroup(FightConnections attackConnection, FightGroups attackGroup, FightGroups beattackGroup)
        {
            if (attackConnection == FightConnections.Opposite)
            {
                if (attackGroup == FightGroups.A) return beattackGroup == FightGroups.B;
                if (attackGroup == FightGroups.B) return beattackGroup == FightGroups.A;
                return false;
            }
            if (attackConnection == FightConnections.Friend)
            {
                if (attackGroup == FightGroups.A) return beattackGroup == FightGroups.A;
                if (attackGroup == FightGroups.B) return beattackGroup == FightGroups.B;
                return false;
            }
            return false;
        }

        public static BeattackClip FindBeattackClip(Entity entity, BeattackComponent beattackComponent)
        {
            if (beattackComponent.beattackClips.Count <= 0) return null;
            int count = beattackComponent.beattackClips.Count;
            for (int i = 0; i < count; i++)
            {
                if (IsBeattackClipAvailable(entity, beattackComponent.beattackClips[i]))
                {
                    return beattackComponent.beattackClips[i];
                }
            }
            return null;
        }

        public static bool IsBeattackClipAvailable(Entity entity, BeattackClip clip)
        {
            return CheckCondition(entity, clip.condition);
        }

        public static bool CheckCondition(Entity entity, ConditionInfo condition)
        {
            if (condition.type == ConditionTypes.None) return true;
            if (condition.type > ConditionTypes.AttrConditionsBegin && condition.type < ConditionTypes.AttrConditionsEnd)
            {
                AttrComponent attrComponent = entity.getComponent<AttrComponent>();
                if (attrComponent == null) return false;
                switch (condition.type)
                {
                    case ConditionTypes.AttrEqual: return attrComponent.getAttr((AttrTypes)condition.valueA) == condition.valueB;
                    case ConditionTypes.AttrNotEqual: return attrComponent.getAttr((AttrTypes)condition.valueA) != condition.valueB;
                    case ConditionTypes.AttrLess: return attrComponent.getAttr((AttrTypes)condition.valueA) < condition.valueB;
                    case ConditionTypes.AttrLessEqual: return attrComponent.getAttr((AttrTypes)condition.valueA) <= condition.valueB;
                    case ConditionTypes.AttrGreater: return attrComponent.getAttr((AttrTypes)condition.valueA) > condition.valueB;
                    case ConditionTypes.AttrGreaterEqual: return attrComponent.getAttr((AttrTypes)condition.valueA) >= condition.valueB;
                }
            }
            return false;
        }
    }
}
