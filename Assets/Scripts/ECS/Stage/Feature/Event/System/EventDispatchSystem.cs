using ECSlike;

namespace Mariolike
{
    public class EventDispatchSystem : EntitySystem
    {
        protected sealed override System.Type[] getWantedTypes()
        {
            return new System.Type[]
            {
                typeof(EventDispatchComponent),
                typeof(EventListenerComponent),
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
            EventDispatchComponent eventDispatchComponent = entity.getComponent<EventDispatchComponent>();
            if (eventDispatchComponent.iNextDispatchEvents.Count > 0)
            {
                EventListenerComponent eventListenerComponent = entity.getComponent<EventListenerComponent>();
                if (eventListenerComponent.mCurEventListeners.Count > 0)
                {
                    int count = eventDispatchComponent.iNextDispatchEvents.Count;
                    for (int i = 0; i < count; i++)
                    {
                        onDispatchEvent(entity, eventListenerComponent, eventDispatchComponent.iNextDispatchEvents[i]);
                    }
                }
                eventDispatchComponent.iNextDispatchEvents.Clear();
            }
        }

        protected void onDispatchEvent(Entity entity, EventListenerComponent eventListenerComponent, EventDispatchData dispatchData)
        {
            int count = eventListenerComponent.mCurEventListeners.Count;
            for (int i = 0; i < count; i++)
            {
                eventListenerComponent.mCurEventListeners[i].Invoke(dispatchData.type, entity, dispatchData.parameter);
            }
        }
    }
}
