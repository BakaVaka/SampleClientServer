using System.Net;

using Shared;

namespace Server;

public class ServerBase {

    private readonly Settings _settings;
    private readonly IEnumerable<Listener> _listeners;
    private readonly object _syncRoot = new();
    private bool _running = false;
    public ServerBase(Settings settings) {
        _settings = settings;
        _listeners = _settings.Listen.Select(x => new Listener(new IPEndPoint(0, x)));
    }

    public async Task Run(CancellationToken cancellationToken) {
        lock( _syncRoot ) {
            if( _running ) {
                throw new InvalidOperationException();
            }
            _running = true;
        }
        Diagnostic.ServerStarted(this, _settings);
        var tasks = _listeners.Select(x => Task.Run(async() => await x.Run(_settings.Application, cancellationToken)));
        await Task.WhenAll(tasks);
        _running = false;
        Diagnostic.ServerStopped(this);
    }

    public class Settings {
        public IReadOnlyCollection<UInt16> Listen { get; }
        public ConnectionHandlerDelegate Application { get; }
        public Settings(ConnectionHandlerDelegate application, params UInt16[] listen) {
            ArgumentNullException.ThrowIfNull(application, nameof(application));
            ArgumentNullException.ThrowIfNull(listen, nameof(listen));
            Application = application;
            Listen = new HashSet<ushort>(listen);
        }
    }
}
