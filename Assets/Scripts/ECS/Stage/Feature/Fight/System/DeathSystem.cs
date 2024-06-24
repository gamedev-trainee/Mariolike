using ECSlike;

namespace Mariolike
{
    public class DeathSystem : EntitySystem
    {
        protected sealed override System.Type[] getWantedTypes()
        {
            return new System.Type[]
            {
                typeof(DeathComponent),
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
            if (deathComponent.iNextDeathState > FightProgressStates.None)
            {
                deathComponent.mCurDeathState = FightProgressStates.Ready;
                deathComponent.mCurKiller = deathComponent.iNextKiller;
                deathComponent.iNextDeathState = 0;
                deathComponent.iNextKiller = Entity.Null;
            }
            switch (deathComponent.mCurDeathState)
            {
                case FightProgressStates.Ready:
                    {
                        deathComponent.mCurDeathState = FightProgressStates.Running;
                        MoveComponent moveComponent = entity.getComponent<MoveComponent>();
                        if (moveComponent != null)
                        {
                            moveComponent.stopMove();
                        }
                        AnimatorComponent animatorComponent = entity.getComponent<AnimatorComponent>();
                        if (animatorComponent != null)
                        {
                            animatorComponent.setParameter(DeathComponent.DeathStateParameter, (int)DeathComponent.DeathStates.Dying);
                        }
                        if (deathComponent.motion != null)
                        {
                            MotionPlayComponent motionPlayComponent = entity.getOrAddComponent<MotionPlayComponent>();
                            motionPlayComponent.playMotion(deathComponent.motion, deathComponent.mCurKiller, onSelfDeathMotionComplete, deathComponent);
                        }
                        else
                        {
                            deathComponent.mCurDeathState = FightProgressStates.Ending;
                        }
                    }
                    break;
                case FightProgressStates.Ending:
                    {
                        AnimatorComponent animatorComponent = entity.getComponent<AnimatorComponent>();
                        if (animatorComponent != null)
                        {
                            if (animatorComponent.isStateComplete(DeathComponent.DeathStateName))
                            {
                                deathComponent.mCurDeathState = FightProgressStates.End;
                            }
                        }
                        else
                        {
                            deathComponent.mCurDeathState = FightProgressStates.End;
                        }
                    }
                    break;
                case FightProgressStates.End:
                    {
                        entity.addComponent<DisposeComponent>();
                    }
                    break;
            }
        }

        protected void onSelfDeathMotionComplete(object parameter)
        {
            DeathComponent deathComponent = parameter as DeathComponent;
            if (deathComponent != null)
            {
                if (deathComponent.mCurDeathState == FightProgressStates.Running)
                {
                    deathComponent.mCurDeathState = FightProgressStates.Ending;
                }
            }
        }
    }
}
