using ECSlike;

namespace Mariolike
{
    public class DeathComponent : IComponent
    {
        public enum DeathStates
        {
            None,
            Dying,
        }

        public static readonly string DeathStateParameter = "death_state";
        public static readonly string DeathStateName = "death";

        [ConfigField]
        public ConditionInfo condition = new ConditionInfo();
        [ConfigField]
        public MotionClip motion = null;

        //

        public FightProgressStates iNextDeathState = FightProgressStates.None;
        public int iNextKiller = 0;

        //

        public FightProgressStates mCurDeathState = FightProgressStates.None;
        public int mCurKiller = 0;

        //

        public bool startDeath(int killer)
        {
            if (iNextDeathState > FightProgressStates.None || mCurDeathState > FightProgressStates.None) return false;
            iNextDeathState = FightProgressStates.Ready;
            iNextKiller = killer;
            return true;
        }

        public bool isDying()
        {
            return iNextDeathState > FightProgressStates.None || mCurDeathState > FightProgressStates.None;
        }
    }
}
