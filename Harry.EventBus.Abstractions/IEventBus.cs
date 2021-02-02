using Harry.EventBus.Handlers;
using System;

namespace Harry.EventBus
{
    public interface IEventBus
    {
        /// <summary>
        /// 发布事件
        /// </summary>
        IEventBus Publish(IEvent @event, string eventName);

        /// <summary>
        /// 订阅事件
        /// </summary>
        IEventBus Subscribe<T, TH>(string eventName)
            where T : IEvent
            where TH : IEventHandler<T>;

        /// <summary>
        /// 订阅动态事件
        /// </summary>
        IEventBus SubscribeDynamic<TH>(string eventName)
            where TH : IDynamicEventHandler;

        /// <summary>
        /// 取消订阅
        /// </summary>
        IEventBus Unsubscribe<T, TH>(string eventName)
            where TH : IEventHandler<T>
            where T : IEvent;

        /// <summary>
        /// 取消动态订阅
        /// </summary>
        IEventBus UnsubscribeDynamic<TH>(string eventName)
            where TH : IDynamicEventHandler;
    }
}
