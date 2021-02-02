using System;
using System.Collections.Generic;
using System.Text;

namespace Harry.EventBus
{
    public static class EventBusFactoryExtensions
    {
        public static IEventBus CreateEventBus(this IEventBusFactory factory)
        {
            if (factory == null)
            {
                throw new ArgumentNullException(nameof(factory));
            }

            return factory.CreateEventBus(string.Empty);
        }
    }
}
