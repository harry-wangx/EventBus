using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Text;

namespace Harry.EventBus.RabbitMQ
{
    public class RabbitMQEventBusProvider : IEventBusProvider
    {
        private readonly string _name;
        public RabbitMQEventBusProvider(string name)
        {
            this._name = name;
        }

        public IEventBus CreateIEventBus(IServiceProvider sp)
        {
            var options = sp.GetRequiredService<IOptionsMonitor<RabbitMQEventBusOptions>>().Get(_name);
            var loggerFactory = sp.GetRequiredService<ILoggerFactory>();

            var connectionFactory = sp.GetService<IConnectionFactory>();
            if (connectionFactory == null)
            {
                var defaultConnectionFactory= new ConnectionFactory();
                //配置ConnectionFactory
                options.Events?.OnCreateConnectionFactory?.Invoke(defaultConnectionFactory);
                connectionFactory = defaultConnectionFactory;
            }

            var connection = new DefaultRabbitMQConnection(connectionFactory, loggerFactory.CreateLogger<DefaultRabbitMQConnection>(), options.RetryCount);

            var manager = sp.GetRequiredService<IEventBusSubscriptionsManager>();

            string queueName = options.QueueDeclare.Name;
            if (string.IsNullOrEmpty(queueName))
            {
                queueName = _name;
            }
            return new RabbitMQEventBus(sp, connection, manager, queueName, options.RetryCount);
        }

        public void Dispose()
        {

        }
    }
}
