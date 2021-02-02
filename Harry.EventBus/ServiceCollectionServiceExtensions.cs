using Harry.EventBus;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionServiceExtensions
    {
        public static IEventBusBuilder AddEventBus(this IServiceCollection services, string name = null)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            services.AddLogging();
            services.AddOptions();

            //每个EventBus有一个管理器
            services.TryAddTransient<IEventBusSubscriptionsManager, InMemoryEventBusSubscriptionsManager>();
            //事件工厂需要注册为单例模式
            services.TryAddSingleton<IEventBusFactory, DefaultEventBusFactory>();
            //默认事件总线
            services.TryAddSingleton<IEventBus>(sp => sp.GetRequiredService<IEventBusFactory>().CreateEventBus());

            if (name == null)
            {
                name = string.Empty;
            }
            return new DefaultEventBusBuilder(services, name);
        }

        public static IServiceCollection AddEventBus(this IServiceCollection services, Action<IEventBusBuilder> builderAction, string name = null)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            var builder = services.AddEventBus(name);

            builderAction?.Invoke(builder);

            return services;
        }
    }
}
