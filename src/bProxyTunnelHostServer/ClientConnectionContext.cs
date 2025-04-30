
using bProxyTunnelCore;
using bProxyTunnelCore.Messages;

using Shared;

namespace bProxyTunnelHostServer;

/// <summary>
/// Client connection context
/// Represent connection in server case scenario
/// </summary>
internal sealed class ClientConnectionContext {

    private Connection _connection;

    public ClientConnectionContext(Connection connection) { 
        _connection = connection;
    }

    public Connection Connection => _connection;
    public bool CloseRequired { get; private set; }
    public void Close() => CloseRequired = true;
    public bool IsHelloMessageReceived { get; set; }
    public string? ClientName { get; set; }
    public string? ClientSecret { get; set; }
    public Request? Request { get; set; }

    public Response? Response { get; private set; }

    public void ACK() {
        Guard.ThrowIfNull(Request);
        Response = Messaging.CreateACK(Request);
    }

    internal void NAK(Int32 errorCode, String errorMessage) {
        Guard.ThrowIfNull(Request);
        Messaging.CreateNAK(Request, errorCode, errorMessage);
    }

    public async Task SendResponse() {
        Guard.ThrowIfNull(Response);
        await _connection.Send(Response, XmlChannel.Encode);
    }
}
