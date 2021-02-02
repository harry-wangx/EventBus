using Harry.EventBus.Handlers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Harry.EventBus.Memory
{
    public class MemoryEventBus : IEventBus
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<MemoryEventBus> _logger;
        private readonly IEventBusSubscriptionsManager _subsManager;

        public MemoryEventBus(IServiceProvider sp, ILoggerFactory loggerFactory, IEventBusSubscriptionsManager subsManager)
        {
            this._serviceProvider = sp;
            this._logger = loggerFactory.CreateLogger<MemoryEventBus>();
            this._subsManager = subsManager;
        }

        /// <summary>
        /// 发布事件
        /// </summary>
        public IEventBus Publish(IEvent @event, string eventName)
        {
            eventName = eventName ?? @event.GetFullName();
            Task.Run(() =>
            {
                ProcessEvent(eventName, @event);
            });
            return this;
        }

        /// <summary>
        /// 订阅事件
        /// </summary>
        public IEventBus Subscribe<T, TH>(string eventName)
            where T : IEvent
            where TH : IEventHandler<T>
        {
            eventName = eventName ?? _subsManager.GetEventKey<T>();
            _logger.LogInformation("Subscribing to event {EventName} with {EventHandler}", eventName, typeof(TH).GetGenericTypeName());

            _subsManager.AddSubscription<T, TH>(eventName);
            return this;
        }

        /// <summary>
        /// 订阅动态事件
        /// </summary>
        public IEventBus SubscribeDynamic<TH>(string eventName)
            where TH : IDynamicEventHandler
        {
            _logger.LogInformation("Subscribing to dynamic event {EventName} with {EventHandler}", eventName, typeof(TH).GetGenericTypeName());

            _subsManager.AddDynamicSubscription<TH>(eventName);
            return this;
        }

        /// <summary>
        /// 取消订阅
        /// </summary>
        public IEventBus Unsubscribe<T, TH>(string eventName)
            where T : IEvent
            where TH : IEventHandler<T>
        {
            eventName = eventName ?? _subsManager.GetEventKey<T>();

            _logger.LogInformation("Unsubscribing from event {EventName}", eventName);

            _subsManager.RemoveSubscription<T, TH>(eventName);
            return this;
        }

        /// <summary>
        /// 取消动态订阅
        /// </summary>
        public IEventBus UnsubscribeDynamic<TH>(string eventName)
            where TH : IDynamicEventHandler
        {
            _subsManager.RemoveDynamicSubscription<TH>(eventName);
            return this;
        }

        private void ProcessEvent(string eventName, IEvent eventData)
        {
            _logger.LogTrace("Processing RabbitMQ event: {EventName}", eventName);

            if (_subsManager.HasSubscriptionsForEvent(eventName))
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var subscriptions = _subsManager.GetHandlersForEvent(eventName);
                    foreach (var subscription in subscriptions)
                    {
                        try
                        {
                            if (subscription.IsDynamic)
                            {
                                if (!(scope.ServiceProvider.GetService(subscription.HandlerType) is IDynamicEventHandler handler)) continue;

                                handler.Handle(eventData);
                            }
                            else
                            {
                                if (eventData.GetType() != subscription.EventType)
                                {
                                    _logger.LogWarning($"发布事件类型与订阅事件类型不符.发布事件类型:{eventData.GetType()} 订阅handler类型:{subscription.HandlerType} 订阅事件类型:{subscription.EventType}");
                                    return;
                                }

                                var handler = scope.ServiceProvider.GetService(subscription.HandlerType);
                                if (handler == null) continue;
                                var concreteType = typeof(IEventHandler<>).MakeGenericType(subscription.EventType);

                                concreteType.GetMethod("Handle").Invoke(handler, new object[] { eventData });
                            }
                        }
                        catch (Exception ex)
                        {
                            _logger.LogError(ex, $"调用订阅处理Handler时出错. EventType:{subscription.EventType.ToString()} HandlerType:{subscription.HandlerType.ToString()}");
                        }
                    }
                }
            }
            else
            {
                _logger.LogWarning("No subscription for RabbitMQ event: {EventName}", eventName);
            }
        }
    }
}
