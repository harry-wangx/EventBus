using System;
using System.Collections.Generic;

namespace Harry.EventBus
{
    public class EventBusFactoryOptions
    {
        public IEventBusProvider EventBusProvider { get; set; }

        public List<Action<IEventBus>> EventBusInitActions { get; } = new List<Action<IEventBus>>();

    }
}
