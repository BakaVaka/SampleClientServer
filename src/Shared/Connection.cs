using System.Net;
using System.Net.Sockets;

namespace Shared;

public sealed class Connection : IDisposable, IAsyncDisposable {

    private readonly NetworkStream _stream;
    private Connection(Socket connection) {
        _stream = new NetworkStream(connection, true);
        Name = "";
    }

    public string Name { get; set; }

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
        return decoder(_stream, cancellationToken);
    }

    public async Task Send<T>(T message, Func<T, ReadOnlyMemory<byte>> encoder) {
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
