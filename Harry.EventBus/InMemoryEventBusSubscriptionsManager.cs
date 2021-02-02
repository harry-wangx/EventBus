using Harry.EventBus.Handlers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Harry.EventBus
{
    public class InMemoryEventBusSubscriptionsManager : IEventBusSubscriptionsManager
    {
        private readonly Dictionary<string, List<SubscriptionInfo>> _handlers;

        public event EventHandler<string> OnEventRemoved;

        public InMemoryEventBusSubscriptionsManager()
        {
            _handlers = new Dictionary<string, List<SubscriptionInfo>>();
        }

        public bool IsEmpty => !_handlers.Keys.Any();

        public void Clear() => _handlers.Clear();

        /// <summary>
        /// 添加订阅信息
        /// </summary>
        public void AddSubscription<T, TH>(string eventName)
            where T : IEvent
            where TH : IEventHandler<T>
        {
            DoAddSubscription(typeof(T), typeof(TH), eventName, isDynamic: false);
        }

        /// <summary>
        /// 添加动态订阅信息
        /// </summary>
        public void AddDynamicSubscription<TH>(string eventName)
            where TH : IDynamicEventHandler
        {
            DoAddSubscription(null, typeof(TH), eventName, isDynamic: true);
        }

        //添加订阅信息
        private void DoAddSubscription(Type eventType, Type handlerType, string eventName, bool isDynamic)
        {
            if (!HasSubscriptionsForEvent(eventName))
            {
                _handlers.Add(eventName, new List<SubscriptionInfo>());
            }

            //一个种类Handler只允许添加一次
            if (_handlers[eventName].Any(s => s.HandlerType == handlerType && s.IsDynamic == isDynamic))
            {
                return;
            }

            if (isDynamic)
            {
                _handlers[eventName].Add(SubscriptionInfo.Dynamic(eventType, handlerType));
            }
            else
            {
                if (eventType == null)
                {
                    throw new ArgumentNullException("订阅事件时,缺少事件类型", nameof(eventType));
                }
                _handlers[eventName].Add(SubscriptionInfo.Typed(eventType, handlerType));
            }
        }

        /// <summary>
        /// 移除订阅
        /// </summary>
        public void RemoveSubscription<T, TH>(string eventName)
            where TH : IEventHandler<T>
            where T : IEvent
        {
            DoRemoveHandler(eventName, typeof(TH), false);
        }

        /// <summary>
        /// 移除动态订阅
        /// </summary>
        public void RemoveDynamicSubscription<TH>(string eventName)
            where TH : IDynamicEventHandler
        {
            DoRemoveHandler(eventName, typeof(TH), true);
        }

        private void DoRemoveHandler(string eventName, Type handlerType, bool isDynamic)
        {
            if (!HasSubscriptionsForEvent(eventName))
            {
                return;
            }

            var subsToRemove = _handlers[eventName].SingleOrDefault(s => s.HandlerType == handlerType && s.IsDynamic == isDynamic);

            if (subsToRemove != null)
            {
                _handlers[eventName].Remove(subsToRemove);
                if (!_handlers[eventName].Any())
                {
                    _handlers.Remove(eventName);
                    RaiseOnEventRemoved(eventName);
                }

            }
        }

        private void RaiseOnEventRemoved(string eventName)
        {
            var handler = OnEventRemoved;
            handler?.Invoke(this, eventName);
        }


        /// <summary>
        /// 是否已有此名称的订阅
        /// </summary>
        public bool HasSubscriptionsForEvent(string eventName) => _handlers.ContainsKey(eventName);

        /// <summary>
        /// 获取订阅列表
        /// </summary>
        public IEnumerable<SubscriptionInfo> GetHandlersForEvent(string eventName) => _handlers[eventName];

        /// <summary>
        /// 
        /// </summary>
        public string GetEventKey<T>()
        {
            return typeof(T).GetFullName();
        }
    }
}
