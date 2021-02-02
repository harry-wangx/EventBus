using Microsoft.Extensions.DependencyInjection;

namespace Harry.EventBus
{
    internal class DefaultEventBusBuilder : IEventBusBuilder
    {
        public DefaultEventBusBuilder(IServiceCollection services, string name)
        {
            Services = services;
            Name = name;
        }

        public string Name { get; }

        public IServiceCollection Services { get; }
    }
}
