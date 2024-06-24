using ECSlike;

namespace Mariolike
{
    public class BeattackActionPlayer : ActionPlayer
    {
        private BeattackActionClip clip => getData<BeattackActionClip>();

        public override void play()
        {
            base.play();

            Entity attacker = getTrigger();
            BeattackComponent beattackComponent = getOrAddComponent<BeattackComponent>();
            BeattackClip beattackClip = FightUtils.FindBeattackClip(getEntity(), beattackComponent);
            if (beattackClip != null)
            {
                if (beattackClip.counterMotion != null)
                {
                    MotionPlayComponent attackerMotionPlayComponent = attacker.getOrAddComponent<MotionPlayComponent>();
                    attackerMotionPlayComponent.playMotion(beattackClip.counterMotion, getEntity());
                }
            }
            beattackComponent.startBeattack(beattackClip, attacker);
            setDone();
        }
    }
}
