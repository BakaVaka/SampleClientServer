using System.Net;
using System.Net.Sockets;

namespace Shared;

/**
 * Класс соединения
 * Используется как клиентом, так и сервером
 */
public sealed class Connection : IDisposable, IAsyncDisposable {

    private readonly NetworkStream _stream;
    private Connection(Socket connection) {
        _stream = new NetworkStream(connection, true);
    }

    public EndPoint? LocalEndPoint => _stream.Socket.LocalEndPoint;
    public EndPoint? RemoteEndPoint => _stream.Socket.RemoteEndPoint;

    public void Close() => _stream.Close();

    public static async Task<Connection> ConnectAsync(IPEndPoint host) {
        var socket = new Socket(host.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
        try {
            await socket.ConnectAsync(host);
            return new Connection(socket);
        } catch(SocketException) {
            socket.Dispose();
            throw;
        }
    }

    public static Connection Connect(Socket socket) => new Connection(socket);

    public Task<T> Receive<T>(Func<Stream, CancellationToken, Task<T>> decoder, CancellationToken cancellationToken = default) {
        // собственно, т.к. мы не знаем пока что что хотим получать
        // нам нужен фильтр который будет из потока байт возвращать сообщение
        return decoder(_stream, cancellationToken);
    }

    public async Task Send<T>(T message, Func<T, ReadOnlyMemory<byte>> encoder) {
        // аналогично методу receive соединение не знает что там будут отправлять
        // но мы хотим отправлять объектики, а потому - 
        var bytes = encoder(message);
        await _stream.WriteAsync(bytes);
    }

    public async ValueTask DisposeAsync() {
        await _stream.DisposeAsync();
    }
    public void Dispose() {
        _stream.Dispose();
    }
}
