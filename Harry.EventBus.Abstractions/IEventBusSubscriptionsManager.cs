using Harry.EventBus.Handlers;
using System;
using System.Collections.Generic;

namespace Harry.EventBus
{
    public interface IEventBusSubscriptionsManager
    {
        bool IsEmpty { get; }
        void Clear();

        event EventHandler<string> OnEventRemoved;

        /// <summary>
        /// 添加订阅信息
        /// </summary>
        void AddSubscription<T, TH>(string eventName)
           where T : IEvent
           where TH : IEventHandler<T>;

        /// <summary>
        /// 添加动态订阅信息
        /// </summary>
        void AddDynamicSubscription<TH>(string eventName)
            where TH : IDynamicEventHandler;

        /// <summary>
        /// 移除订阅
        /// </summary>
        void RemoveSubscription<T, TH>(string eventName)
             where TH : IEventHandler<T>
             where T : IEvent;

        /// <summary>
        /// 移除动态订阅
        /// </summary>
        void RemoveDynamicSubscription<TH>(string eventName)
            where TH : IDynamicEventHandler;

        /// <summary>
        /// 是否已有此名称的订阅
        /// </summary>
        bool HasSubscriptionsForEvent(string eventName);

        /// <summary>
        /// 获取订阅列表
        /// </summary>
        IEnumerable<SubscriptionInfo> GetHandlersForEvent(string eventName);

        /// <summary>
        /// 
        /// </summary>
        string GetEventKey<T>();
    }
}