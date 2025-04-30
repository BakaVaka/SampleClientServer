using Microsoft.Extensions.Logging;

using Shared;

namespace bProxyTunnelHostServer;
public static class Handlers {
    internal static async Task ExceptionHandler(Connection connection, ConnectionHandlerDelegate? next, CancellationToken cancellationToken) {
        try {
            if(next is not null) {
                await next(connection, cancellationToken).ConfigureAwait(false);
            }
        }
        catch(Exception ex) {
            Program.Logger.LogError(ex, "Error during processing connection; Error: {ex}", ex);
        }
    }

    internal static ConnectionHandlerDelegate LogConnectionAccepted(ILogger logger)
        => LogConnectionInfo(logger, (con) => $"Connection accepted; Connection: {con.Name}; RemoteEndPoint: {con.RemoteEndPoint}");

    internal static ConnectionHandlerDelegate LogConnectionComplete(ILogger logger)
        => LogConnectionInfo(logger, (con) => $"Connection complete; Connection: {con.Name}; RemoteEndPoint: {con.RemoteEndPoint}");

    public static ConnectionHandlerDelegate LogConnectionInfo(ILogger logger, Func<Connection, string> msgProvider) {
        return (connection, cancellationToken) => { 
            logger.LogInformation($"{msgProvider(connection)}");
            return Task.CompletedTask;
        };
    }

    internal static async Task RunBaseApplicationLoop(Connection connection, CancellationToken cancellationToken) {

    }
}
