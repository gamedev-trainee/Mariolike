using ECSlike;

namespace Mariolike
{
    public class BeattackSystem : EntitySystem
    {
        protected sealed override System.Type[] getWantedTypes()
        {
            return new System.Type[]
            {
                typeof(BeattackComponent),
            };
        }

        protected override System.Type[] getUnwantedTypes()
        {
            return new System.Type[]
            {
                typeof(DisposeComponent),
            };
        }

        protected override void onUpdateEntity(World world, int entity)
        {
            DeathComponent deathComponent = world.getComponent<DeathComponent>(entity);
            if (deathComponent != null && deathComponent.isDying())
            {
                return;
            }
            BeattackComponent beattackComponent = world.getComponent<BeattackComponent>(entity);
            if (beattackComponent.iNextBeattackState > FightProgressStates.None)
            {
                beattackComponent.mCurBeattackState = FightProgressStates.Ready;
                beattackComponent.mCurBeattackClip = beattackComponent.iNextBeattackClip;
                beattackComponent.mCurAttacker = beattackComponent.iNextAttacker;
                beattackComponent.iNextBeattackState = 0;
                beattackComponent.iNextBeattackClip = null;
                beattackComponent.iNextAttacker = 0;
            }
            switch (beattackComponent.mCurBeattackState)
            {
                case FightProgressStates.Ready:
                    {
                        beattackComponent.mCurBeattackState = FightProgressStates.Running;
                        MoveComponent moveComponent = world.getComponent<MoveComponent>(entity);
                        if (moveComponent != null)
                        {
                            moveComponent.stopMove();
                        }
                        if (beattackComponent.mCurBeattackClip != null)
                        {
                            AnimatorComponent animatorComponent = world.getComponent<AnimatorComponent>(entity);
                            if (animatorComponent != null)
                            {
                                animatorComponent.setParameter(BeattackComponent.BeattackStateParameter, (int)BeattackComponent.BeattackStates.Beattacking);
                            }
                            if (beattackComponent.mCurBeattackClip.motion != null)
                            {
                                MotionPlayComponent motionPlayComponent = world.getOrAddComponent<MotionPlayComponent>(entity);
                                motionPlayComponent.playMotion(beattackComponent.mCurBeattackClip.motion, beattackComponent.mCurAttacker, onBeattackMotionComplete, beattackComponent);
                            }
                            else
                            {
                                beattackComponent.mCurBeattackState = FightProgressStates.Ending;
                            }
                        }
                        else
                        {
                            beattackComponent.mCurBeattackState = FightProgressStates.End;
                        }
                    }
                    break;
                case FightProgressStates.Ending:
                    {
                        AnimatorComponent animatorComponent = world.getComponent<AnimatorComponent>(entity);
                        if (animatorComponent != null)
                        {
                            if (animatorComponent.isStateComplete(BeattackComponent.BeattackStateName))
                            {
                                beattackComponent.mCurBeattackState = FightProgressStates.End;
                            }
                        }
                        else
                        {
                            beattackComponent.mCurBeattackState = FightProgressStates.End;
                        }
                    }
                    break;
                case FightProgressStates.End:
                    {
                        int attacker = beattackComponent.mCurAttacker;
                        beattackComponent.mCurBeattackState = FightProgressStates.None;
                        beattackComponent.mCurAttacker = 0;
                        AnimatorComponent animatorComponent = world.getComponent<AnimatorComponent>(entity);
                        if (animatorComponent != null)
                        {
                            animatorComponent.setParameter(BeattackComponent.BeattackStateParameter, (int)BeattackComponent.BeattackStates.None);
                        }
                        if (deathComponent != null)
                        {
                            if (FightUtils.CheckCondition(world, entity, deathComponent.condition))
                            {
                                deathComponent.startDeath(attacker);
                            }
                        }
                    }
                    break;
            }
        }

        protected void onBeattackMotionComplete(object parameter)
        {
            BeattackComponent beattackComponent = parameter as BeattackComponent;
            if (beattackComponent != null)
            {
                if (beattackComponent.mCurBeattackState == FightProgressStates.Running)
                {
                    beattackComponent.mCurBeattackState = FightProgressStates.Ending;
                }
            }
        }
    }
}
