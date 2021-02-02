using Microsoft.Extensions.DependencyInjection;
using System;

namespace Harry.EventBus.Memory
{
    public class MemoryEventBusProvider : IEventBusProvider
    {
        //private readonly string _name;
        //public MemoryEventBusProvider(string name)
        //{
        //    this._name = name;
        //}

        public IEventBus CreateIEventBus(IServiceProvider serviceProvider)
        {
            return serviceProvider.GetRequiredService<MemoryEventBus>();
        }

        public void Dispose()
        {

        }
    }
}
