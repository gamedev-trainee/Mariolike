namespace Mariolike
{
    public class EventDispatchActionPlayer : ActionPlayer
    {
        private EventDispatchActionClip clip => getData<EventDispatchActionClip>();

        public override void play()
        {
            base.play();

            EventDispatchComponent eventDispatchComponent = getOrAddComponent<EventDispatchComponent>();
            int count = clip.events.Count;
            for (int i = 0; i < count; i++)
            {
                eventDispatchComponent.sendEvent(clip.events[i].type);
            }
            setDone();
        }
    }
}
