using Microsoft.Extensions.DependencyInjection;
using System;

namespace Harry.EventBus
{
    public static class EventBusBuilderExtensions
    {
        public static IEventBusBuilder Configure<TOptions>(this IEventBusBuilder builder, Action<TOptions> configureOptions) where TOptions : class
        {
            if (builder == null)
                throw new ArgumentNullException(nameof(builder));

            builder.Services.Configure(builder.Name, configureOptions);

            return builder;
        }

        public static IEventBusBuilder InitEventBus(this IEventBusBuilder builder, Action<IEventBus> configureEventBus)
        {
            if (builder == null)
                throw new ArgumentNullException(nameof(builder));

            if (configureEventBus != null)
            {
                builder.Configure<EventBusFactoryOptions>(options => options.EventBusInitActions.Add(configureEventBus));
            }

            return builder;
        }
    }
}
