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

        protected override void onUpdateEntity(Entity entity)
        {
            DeathComponent deathComponent = entity.getComponent<DeathComponent>();
            if (deathComponent != null && deathComponent.isDying())
            {
                return;
            }
            BeattackComponent beattackComponent = entity.getComponent<BeattackComponent>();
            if (beattackComponent.iNextBeattackState > FightProgressStates.None)
            {
                beattackComponent.mCurBeattackState = FightProgressStates.Ready;
                beattackComponent.mCurBeattackClip = beattackComponent.iNextBeattackClip;
                beattackComponent.mCurAttacker = beattackComponent.iNextAttacker;
                beattackComponent.iNextBeattackState = 0;
                beattackComponent.iNextBeattackClip = null;
                beattackComponent.iNextAttacker = Entity.Null;
            }
            switch (beattackComponent.mCurBeattackState)
            {
                case FightProgressStates.Ready:
                    {
                        beattackComponent.mCurBeattackState = FightProgressStates.Running;
                        MoveComponent moveComponent = entity.getComponent<MoveComponent>();
                        if (moveComponent != null)
                        {
                            moveComponent.stopMove();
                        }
                        if (beattackComponent.mCurBeattackClip != null)
                        {
                            AnimatorComponent animatorComponent = entity.getComponent<AnimatorComponent>();
                            if (animatorComponent != null)
                            {
                                animatorComponent.setParameter(BeattackComponent.BeattackStateParameter, (int)BeattackComponent.BeattackStates.Beattacking);
                            }
                            if (beattackComponent.mCurBeattackClip.motion != null)
                            {
                                MotionPlayComponent motionPlayComponent = entity.getOrAddComponent<MotionPlayComponent>();
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
                        AnimatorComponent animatorComponent = entity.getComponent<AnimatorComponent>();
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
                        Entity attacker = beattackComponent.mCurAttacker;
                        beattackComponent.mCurBeattackState = FightProgressStates.None;
                        beattackComponent.mCurAttacker = Entity.Null;
                        AnimatorComponent animatorComponent = entity.getComponent<AnimatorComponent>();
                        if (animatorComponent != null)
                        {
                            animatorComponent.setParameter(BeattackComponent.BeattackStateParameter, (int)BeattackComponent.BeattackStates.None);
                        }
                        if (deathComponent != null)
                        {
                            if (FightUtils.CheckCondition(entity, deathComponent.condition))
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
