using ECSlike;

namespace Mariolike
{
    public class MotionPlaySystem : EntitySystem
    {
        protected sealed override System.Type[] getWantedTypes()
        {
            return new System.Type[]
            {
                typeof(MotionPlayComponent),
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
            MotionPlayComponent motionPlayComponent = entity.getComponent<MotionPlayComponent>();
            if (motionPlayComponent.iCurMotionData != null)
            {
                motionPlayComponent.iCurMotionPlayer.update();
                if (motionPlayComponent.iCurMotionPlayer.isDone())
                {
                    motionPlayComponent.iCurMotionPlayer.dispose();
                    motionPlayComponent.iCurMotionPlayer = null;
                    motionPlayComponent.iCurMotionData.callback?.Invoke(motionPlayComponent.iCurMotionData.parameter);
                    motionPlayComponent.iCurMotionData = null;
                }
                return;
            }
            if (motionPlayComponent.iNextMotions.Count > 0)
            {
                MotionPlayData playData = motionPlayComponent.iNextMotions[0];
                motionPlayComponent.iNextMotions.RemoveAt(0);
                motionPlayComponent.iCurMotionData = playData;
                motionPlayComponent.iCurMotionPlayer = new MotionPlayer();
                motionPlayComponent.iCurMotionPlayer.setEntity(entity);
                motionPlayComponent.iCurMotionPlayer.setTrigger(playData.trigger.isNull() ? entity : playData.trigger);
                motionPlayComponent.iCurMotionPlayer.setData(playData.motion);
                motionPlayComponent.iCurMotionPlayer.play();
            }
        }
    }
}
