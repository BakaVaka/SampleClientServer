using System.Net;
using System.Net.Sockets;

using Shared;

namespace Server;

/*
 * Класс "слушателя"
 * Принимает входящее соединение и выполняет "приложение" над подключением
 */
public sealed class Listener {

    private Socket? _server;
    private readonly IPEndPoint _listenerEndPoint;
    private object _suncRoot = new();
    public Listener(IPEndPoint listenerEndPoint) {
        _listenerEndPoint = listenerEndPoint;
    }

    public bool IsRunning => _server is not null;
    public EndPoint Listen => _listenerEndPoint;

    public async Task Run(ConnectionHandlerDelegate application, CancellationToken cancellationToken) {
        lock( _suncRoot ) {
            if(_server is not null ) {
                throw new InvalidOperationException();
            }
            _server = CreateServerSocket();
        }

        while( !cancellationToken.IsCancellationRequested ) {
            try {
                
                var client = await _server!.AcceptAsync(cancellationToken);
                if( client != null ) {
                    var connection = Connection.Connect(client);
                    ThreadPool.UnsafeQueueUserWorkItem(async (state) => {
                        var connection = state as Connection;
                        try {
                            await application(connection, cancellationToken);
                        }
                        catch(Exception ex) { Console.Error.WriteLine($"{ex}"); }
                        try {
                            if(connection is not null ) {
                                await connection.DisposeAsync();
                            }
                        }
                        catch(Exception ex) { }
                    }, connection);
                }
            }
            // do nothing, this is server call stop
            catch( OperationCanceledException ) {}
        }
        // если нам нужен "рестарт" и мы хотим переиспользовать объект слушаетля
        _server?.Dispose();
        _server = null;
    }

    private Socket? CreateServerSocket() {
        var temp = new Socket(_listenerEndPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
        temp.Bind(_listenerEndPoint);
        temp.Listen();
        return temp;
    }
}
