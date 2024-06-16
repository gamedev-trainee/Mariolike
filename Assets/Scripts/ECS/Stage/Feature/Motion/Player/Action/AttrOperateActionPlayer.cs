namespace Mariolike
{
    public class AttrOperateActionPlayer : ActionPlayer
    {
        private AttrOperateActionClip clip => getData<AttrOperateActionClip>();

        public override void play()
        {
            base.play();

            AttrComponent attrComponent = getComponent<AttrComponent>();
            if (attrComponent != null)
            {
                EventDispatchComponent eventDispatchComponent = getOrAddComponent<EventDispatchComponent>();
                int count = clip.operations.Count;
                for (int i = 0; i < count; i++)
                {
                    attrComponent.operateAttr(clip.operations[i].type, clip.operations[i].operation, clip.operations[i].value);
                    if (!clip.dispatch) continue;
                    eventDispatchComponent.sendEvent(EventTypes.AttrChange, clip.operations[i].type);
                }
            }
            setDone();
        }
    }
}
