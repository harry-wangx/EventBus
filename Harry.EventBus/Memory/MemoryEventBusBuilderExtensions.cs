using Harry.EventBus.Memory;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;

namespace Harry.EventBus
{
    public static class MemoryEventBusBuilderExtensions
    {
        /// <summary>
        /// 使用内存事件总线
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static IEventBusBuilder UseMemory(this IEventBusBuilder builder)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            builder.Services.TryAddTransient<MemoryEventBus>();

            return builder.Configure<EventBusFactoryOptions>(options =>
            {
                options.EventBusProvider = new MemoryEventBusProvider();
            });
        }

    }
}
