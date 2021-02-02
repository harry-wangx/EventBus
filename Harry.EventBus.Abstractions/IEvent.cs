using System;

namespace Harry.EventBus
{
    public interface IEvent
    {
        string Id { get; }

        DateTimeOffset CreationDate { get; }
    }
}
