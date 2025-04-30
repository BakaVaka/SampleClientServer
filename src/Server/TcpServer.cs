using System.Net;

namespace Server;

public class TcpServer {

    private readonly TcpServerSettings _settings;
    private readonly IEnumerable<Listener> _listeners;
    private readonly object _syncRoot = new();
    private bool _running = false;
    public TcpServer(TcpServerSettings settings) {
        _settings = settings;
        _listeners = _settings.ListenPorts.Select(port => new Listener(new IPEndPoint(0, port)));
    }

    public async Task Run(CancellationToken cancellationToken) {
        lock( _syncRoot ) {
            if( _running ) {
                throw new InvalidOperationException();
            }
            _running = true;
        }
        Diagnostic.ServerStarted(_settings);
        var tasks = _listeners.Select(listener => Task.Run(async() => await listener.Run(_settings.Application, cancellationToken)));
        await Task.WhenAll(tasks);
        _running = false;
        Diagnostic.ServerStopped(this);
    }
}
