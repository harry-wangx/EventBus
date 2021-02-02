using Microsoft.Extensions.DependencyInjection;

namespace Harry.EventBus
{
    public interface IEventBusBuilder
    {
        string Name { get; }

        IServiceCollection Services { get; }
    }
}
