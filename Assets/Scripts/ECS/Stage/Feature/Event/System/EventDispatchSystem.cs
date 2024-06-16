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

        protected override void onUpdateEntity(World world, int entity)
        {
            EventDispatchComponent eventDispatchComponent = world.getComponent<EventDispatchComponent>(entity);
            if (eventDispatchComponent.iNextDispatchEvents.Count > 0)
            {
                EventListenerComponent eventListenerComponent = world.getComponent<EventListenerComponent>(entity);
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

        protected void onDispatchEvent(int entity, EventListenerComponent eventListenerComponent, EventDispatchData dispatchData)
        {
            int count = eventListenerComponent.mCurEventListeners.Count;
            for (int i = 0; i < count; i++)
            {
                eventListenerComponent.mCurEventListeners[i].Invoke(dispatchData.type, entity, dispatchData.parameter);
            }
        }
    }
}
