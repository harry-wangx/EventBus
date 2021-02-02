using System;
using System.Collections.Generic;
using System.Text;

namespace Harry.EventBus
{
    public interface IEventBusProvider: IDisposable
    {
        IEventBus CreateIEventBus(IServiceProvider serviceProvider);
    }
}
