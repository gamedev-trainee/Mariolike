using ECSlike;
using System.Collections.Generic;

namespace Mariolike
{
    public class BeattackComponent : IComponent
    {
        public enum BeattackStates
        {
            None,
            Beattacking,
        }

        public static readonly string BeattackStateParameter = "beattack_state";
        public static readonly string BeattackStateName = "beattack";

        [ConfigField]
        public List<BeattackClip> beattackClips = new List<BeattackClip>();
        //

        public FightProgressStates iNextBeattackState = FightProgressStates.None;
        public BeattackClip iNextBeattackClip = null;
        public Entity iNextAttacker = Entity.Null;

        //

        public FightProgressStates mCurBeattackState = FightProgressStates.None;
        public BeattackClip mCurBeattackClip = null;
        public Entity mCurAttacker = Entity.Null;

        //

        public bool startBeattack(BeattackClip clip, Entity attacker)
        {
            if (isBeattacking()) return false;
            iNextBeattackState = FightProgressStates.Ready;
            iNextBeattackClip = clip;
            iNextAttacker = attacker;
            return true;
        }

        public bool isBeattacking()
        {
            return iNextBeattackState > FightProgressStates.None || mCurBeattackState > FightProgressStates.None;
        }
    }
}
