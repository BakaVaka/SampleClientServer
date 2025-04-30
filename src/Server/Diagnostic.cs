using System.Diagnostics;
using System.Net.Sockets;
using System.Security.Cryptography.X509Certificates;

using Shared;

namespace Server;
public static class Diagnostic {

    private static readonly DiagnosticSource _myDiagnosticSource = new DiagnosticListener("Baka.ServerBase");
    public static DiagnosticListener Source => (DiagnosticListener)_myDiagnosticSource;

    
    internal static void ClientAccepted(Socket client) 
        => PublishEvent(DiagnosticEvents.ClientAcceptedEvent, new { client.LocalEndPoint, client.RemoteEndPoint });
    
    internal static void ClientApplicationStarted(Connection connection) 
        => PublishEvent(DiagnosticEvents.ClientApplicationStartedEvent, new { connection.Name, connection.LocalEndPoint, connection.RemoteEndPoint });
    
    internal static void ClientApplicationComplete(Connection connection) 
        => PublishEvent(DiagnosticEvents.ClientApplicationCompleteEvent, new { connection.Name, connection.LocalEndPoint, connection.RemoteEndPoint });
    
    internal static void Error(Exception ex) 
        => PublishEvent(DiagnosticEvents.ErrorEvent, ex);
    
    internal static void ServerStarted(TcpServerSettings settings) 
        => PublishEvent(DiagnosticEvents.ServerStartedEvent, new { LisetnPorts = string.Join(";", settings.ListenPorts) });
    internal static void ServerStopped(TcpServer server) => PublishEvent(DiagnosticEvents.ServerStoppedEvent, server);

    private static void PublishEvent(string e, object parameters) {
        //if( _myDiagnosticSource.IsEnabled("") ) 
        {
            _myDiagnosticSource.Write(e, parameters);
        }
    }
}

public static class DiagnosticEvents {
    public static string ClientAcceptedEvent = "ClientAccepted";
    public static string ClientApplicationStartedEvent = "ClientApplicationStarted";
    public static string ClientApplicationCompleteEvent = "ClientApplicationComplete";
    public static string ErrorEvent = "Error";
    public static string ServerStartedEvent = "ServerStarted";
    public static string ServerStoppedEvent = "ServerStopped";
}