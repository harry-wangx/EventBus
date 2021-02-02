using Harry.EventBus;

namespace Sample.Events
{
    public class TestEvent : Event
    {
        public string Data { get; set; }
    }
}
