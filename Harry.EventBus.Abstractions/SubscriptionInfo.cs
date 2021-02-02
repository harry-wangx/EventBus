using System;

namespace Harry.EventBus
{
    public class SubscriptionInfo
    {
        public bool IsDynamic { get; }

        public Type EventType { get; }

        public Type HandlerType { get; }

        private SubscriptionInfo(bool isDynamic, Type eventType, Type handlerType)
        {
            IsDynamic = isDynamic;
            EventType = eventType;
            HandlerType = handlerType;
        }

        public static SubscriptionInfo Dynamic(Type eventType, Type handlerType)
        {
            return new SubscriptionInfo(true, eventType, handlerType);
        }
        public static SubscriptionInfo Typed(Type eventType, Type handlerType)
        {
            return new SubscriptionInfo(false, eventType, handlerType);
        }
    }
}
