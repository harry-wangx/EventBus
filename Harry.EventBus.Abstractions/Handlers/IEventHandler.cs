using System.Threading.Tasks;

namespace Harry.EventBus.Handlers
{
    public interface IEventHandler<in TEvent> : IEventHandler
        where TEvent : IEvent
    {
        Task Handle(TEvent eventData);
    }

    public interface IEventHandler { }
}
