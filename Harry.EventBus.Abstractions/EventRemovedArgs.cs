using System;
using System.Collections.Generic;
using System.Text;

namespace Harry.EventBus
{
    public class EventRemovedArgs : EventArgs
    {
        public EventRemovedArgs(string eventName)
        {
            this.EventName = eventName;
        }
        public string EventName { get; private set; }
    }
}
