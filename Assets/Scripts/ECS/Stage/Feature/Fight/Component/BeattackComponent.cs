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
        public int iNextAttacker = 0;

        //

        public FightProgressStates mCurBeattackState = FightProgressStates.None;
        public BeattackClip mCurBeattackClip = null;
        public int mCurAttacker = 0;

        //

        public bool startBeattack(BeattackClip clip, int attacker)
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
