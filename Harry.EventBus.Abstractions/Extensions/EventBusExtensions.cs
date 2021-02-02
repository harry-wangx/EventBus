using Harry.EventBus.Handlers;

namespace Harry.EventBus
{
    public static class EventBusExtensions
    {
        /// <summary>
        /// 发布事件
        /// </summary>
        public static IEventBus Publish(this IEventBus bus, IEvent @event)
        {
            return bus.Publish(@event, null);
        }

        /// <summary>
        /// 订阅事件
        /// </summary>
        public static IEventBus Subscribe<T, TH>(this IEventBus bus)
            where T : IEvent
            where TH : IEventHandler<T>
        {
            return bus.Subscribe<T, TH>(null);
        }

        /// <summary>
        /// 取消订阅
        /// </summary>
        public static IEventBus Unsubscribe<T, TH>(this IEventBus bus)
            where T : IEvent
            where TH : IEventHandler<T>
        {
            return bus.Unsubscribe<T, TH>(null);
        }
    }
}
