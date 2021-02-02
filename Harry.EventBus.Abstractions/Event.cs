using System;

namespace Harry.EventBus
{
    public abstract class Event : IEvent
    {
        public string Id { get; protected set; } = Guid.NewGuid().ToString();

        public DateTimeOffset CreationDate { get; protected set; } = DateTimeOffset.UtcNow;
    }
}
