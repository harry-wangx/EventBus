using System;
using System.Collections.Generic;
using System.Text;

namespace Harry.EventBus
{
    public interface IEventBusFactory:IDisposable
    {
        IEventBus CreateEventBus(string name);
    }
}
