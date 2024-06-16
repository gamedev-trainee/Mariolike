namespace Mariolike
{
    public class BeattackActionPlayer : ActionPlayer
    {
        private BeattackActionClip clip => getData<BeattackActionClip>();

        public override void play()
        {
            base.play();

            int attacker = getTrigger();
            BeattackComponent beattackComponent = getOrAddComponent<BeattackComponent>();
            BeattackClip beattackClip = FightUtils.FindBeattackClip(getWorld(), getEntity(), beattackComponent);
            if (beattackClip != null)
            {
                if (beattackClip.counterMotion != null)
                {
                    MotionPlayComponent attackerMotionPlayComponent = getWorld().getOrAddComponent<MotionPlayComponent>(attacker);
                    attackerMotionPlayComponent.playMotion(beattackClip.counterMotion, getEntity());
                }
            }
            beattackComponent.startBeattack(beattackClip, attacker);
            setDone();
        }
    }
}
