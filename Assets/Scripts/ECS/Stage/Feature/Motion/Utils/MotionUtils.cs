namespace Mariolike
{
    public class MotionUtils
    {
        public static ActionPlayer CreateActionPlayer(System.Type type)
        {
            if (typeof(AttrOperateActionClip).IsAssignableFrom(type)) return new AttrOperateActionPlayer();
            if (typeof(BeattackActionClip).IsAssignableFrom(type)) return new BeattackActionPlayer();
            if (typeof(EventDispatchActionClip).IsAssignableFrom(type)) return new EventDispatchActionPlayer();
            if (typeof(GenerateActionClip).IsAssignableFrom(type)) return new GenerateActionPlayer();
            if (typeof(MoveBackwardActionClip).IsAssignableFrom(type)) return new MoveBackwardActionPlayer();
            if (typeof(MoveToUIAttrActionClip).IsAssignableFrom(type)) return new MoveToUIAttrActionPlayer();
            if (typeof(PlayMotionActionClip).IsAssignableFrom(type)) return new PlayMotionActionPlayer();
            if (typeof(ScaleJumpSpeedActionClip).IsAssignableFrom(type)) return new ScaleJumpSpeedActionPlayer();
            if (typeof(ScaleActionClip).IsAssignableFrom(type)) return new ScaleActionPlayer();
            if (typeof(ActionCollectionClip).IsAssignableFrom(type)) return new ActionCollectionPlayer();
            return null;
        }
    }
}
