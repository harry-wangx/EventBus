using Harry.EventBus;
using Harry.EventBus.RabbitMQ;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Text;

namespace Harry.EventBus
{
    public static class RabbitMQEventBusBuilderExtensions
    {

        public static IEventBusBuilder UseRabbitMQ(this IEventBusBuilder builder, Action<RabbitMQEventBusOptions> optionsAction = null)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            if (optionsAction != null)
                builder.Configure(optionsAction);

            return builder.Configure<EventBusFactoryOptions>(options =>
            {
                options.EventBusProvider = new RabbitMQEventBusProvider(builder.Name);
            });
        }

    }
}
