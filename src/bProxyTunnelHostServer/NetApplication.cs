
using bProxyTunnelCore.Messages;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using Shared;

namespace bProxyTunnelHostServer;
public sealed class NetApplication {

    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<NetApplication> _logger;

    public NetApplication(IServiceScopeFactory scopeFactory, ILogger<NetApplication> logger) {
        _scopeFactory = scopeFactory;
        _logger = logger;
    }

    public async Task RunBaseApplication(Connection connection, CancellationToken cancellationToken) {


        var context = new ClientConnectionContext(connection);
        while(!cancellationToken.IsCancellationRequested) {
            context.Request = null;
            var msg = await connection.Receive(XmlChannel.Decode, cancellationToken);

            if(msg is null) {
                _logger.LogWarning("No message received; Connection will be dropped");
                return;
            }

            switch(msg) {
                case Request request:

                    await HandleRequest(context, request, cancellationToken);
                    if(context.CloseRequired)
                        return;

                    break;
                case Ping ping:
                    await connection.Send(ping, XmlChannel.Encode);
                    break;
                default:
                    _logger.LogWarning("Unsupported message type");
                    break;
            }
        }
    }

    private async Task HandleRequest(ClientConnectionContext connection, Request request, CancellationToken cancellationToken) {
        switch(request.Meta.Type) {
            case (Consts.HelloRequest): 
                {
                    await ProcessHelloRequest(connection, request, cancellationToken);
                    break;
                }
        }
    }

    private async Task ProcessHelloRequest(ClientConnectionContext connection, Request request, CancellationToken cancellationToken) {
        if(connection.IsHelloMessageReceived) {
            connection.NAK(0xF0, "Already connected");
            await connection.SendResponse();
            return;
        }

        var clientName = request.Headers.Single(x=>x.Type == Consts.ClientNameHeader);
        var clientSecret = request.Headers.Single(x=>x.Type == Consts.ClientSecretHeader);

        if(string.IsNullOrWhiteSpace(clientName.Type) || string.IsNullOrWhiteSpace(clientSecret.Type)) {
            connection.NAK(0xF1, "Required headers missing");
            await connection.SendResponse();
            return;
        }
    }
}
