using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;

namespace Harry.EventBus
{
    public class DefaultEventBusFactory : IEventBusFactory
    {
        private readonly Dictionary<string, IEventBus> _eventBus = new Dictionary<string, IEventBus>();
        private readonly IOptionsMonitor<EventBusFactoryOptions> _optionsMonitor;
        private readonly IServiceProvider _serviceProvider;
        private readonly object _sync = new object();
        private volatile bool _disposed;

        public DefaultEventBusFactory(IServiceProvider sp, IOptionsMonitor<EventBusFactoryOptions> optionsMonitor)
        {
            _serviceProvider = sp;
            _optionsMonitor = optionsMonitor;
        }

        public IEventBus CreateEventBus(string name)
        {
            if (CheckDisposed())
            {
                throw new ObjectDisposedException(nameof(DefaultEventBusFactory));
            }

            name = name ?? string.Empty;

            IEventBus result = null;
            if (_eventBus.TryGetValue(name, out result))
            {
                return result;
            }

            lock (_sync)
            {
                if (_eventBus.TryGetValue(name, out result))
                {
                    return result;
                }

                result = create(name);
                if (result != null)
                {
                    _eventBus.Add(name, result);
                }
                return result;
            }
        }

        private IEventBus create(string name)
        {
            var options = _optionsMonitor.Get(name);
            var result = options.EventBusProvider?.CreateIEventBus(_serviceProvider);
            if (result != null)
            {
                options.EventBusInitActions.ForEach(action => action.Invoke(result));
            }
            return result;
        }

        protected virtual bool CheckDisposed() => _disposed;

        public void Dispose()
        {
            if (!_disposed)
            {
                _disposed = true;

                //try
                //{

                //}
                //catch { }

                foreach (var item in _eventBus.Values)
                {
                    if (item is IDisposable disposableItem)
                    {
                        try
                        {
                            disposableItem.Dispose();
                        }
                        catch { }
                    }
                }
            }
        }
    }
}
