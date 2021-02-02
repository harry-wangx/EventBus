using System.Threading.Tasks;

namespace Harry.EventBus.Handlers
{
    public interface IDynamicEventHandler
    {
        Task Handle(dynamic eventData);
    }
}
