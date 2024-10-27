using System.Diagnostics;
using System.Net.Sockets;

namespace Server;
internal static class Diagnostic {
    private static readonly DiagnosticSource _myDiagnosticSource = new DiagnosticListener("Baka.ServerBase");

    internal static void ClientAccepted(Socket client) => PublishEvent("ClientAccepted", new { client.LocalEndPoint, client.RemoteEndPoint });
    internal static void Error(Exception ex) => PublishEvent("Error", ex);
    internal static void ServerStarted(ServerBase server, object settings) => PublishEvent("ServerStarted", new { server, settings });
    internal static void ServerStopped(ServerBase server) => PublishEvent("ServerStopped", server);

    private static void PublishEvent(string e, object parameters) {
        if( _myDiagnosticSource.IsEnabled("") ) {
            _myDiagnosticSource.Write(e, parameters);
        }
    }
}
