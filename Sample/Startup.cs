using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Harry.EventBus;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Sample.EventHandling;
using Sample.Events;

namespace Sample
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddTransient<TestEventHandler>();
            services.AddEventBus(b => b
                .UseMemory()
                //.InitEventBus(bus => bus.Subscribe<TestEvent, TestEventHandler>())
                //.InitEventBus(bus => bus.SubscribeDynamic<TestEventHandler>("Sample.Events.TestEvent"))
                .InitEventBus(bus => bus.Subscribe<TestEvent, TestEventHandler>("test"))
                .InitEventBus(bus => bus.SubscribeDynamic<TestEventHandler>("test"))
            );
        }


        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.Map("/send", builder =>
            {
                builder.Run(async (context) =>
                {
                    var @event = new TestEvent() { Data = "Hello World!!!" };
                    //context.RequestServices.GetRequiredService<IEventBusFactory>().CreateEventBus().Publish(@event);
                    context.RequestServices.GetRequiredService<IEventBusFactory>().CreateEventBus().Publish(@event,"test");
                    Console.WriteLine("消息已发送");
                    await context.Response.WriteAsync("Hello World!");
                });
            });
        }
    }
}
