using Harry.EventBus;
using Harry.EventBus.Handlers;
using Sample.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sample.EventHandling
{
    public class TestEventHandler : IEventHandler<TestEvent>, IDynamicEventHandler
    {
        public Task Handle(TestEvent @event)
        {
            return Task.Run(() =>
            {
                System.Threading.Thread.Sleep(1000);
                Console.WriteLine("收到消息:" + Newtonsoft.Json.JsonConvert.SerializeObject(@event));
            });
        }

        public Task Handle(dynamic eventData)
        {
            return Task.Run(() =>
            {
                System.Threading.Thread.Sleep(2000);
                Console.WriteLine("收到动态消息:" + Newtonsoft.Json.JsonConvert.SerializeObject(eventData));
                string data = eventData.Data;

                Console.WriteLine("data:" + data);
            });
        }
    }
}
