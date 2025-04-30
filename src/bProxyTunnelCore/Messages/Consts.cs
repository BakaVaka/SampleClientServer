namespace bProxyTunnelCore.Messages;

public class Consts {

    // ------------------------------------------------------------------------
    // MessageTypes
    // ------------------------------------------------------------------------
    public const string HelloRequest = "RequestHello";
    public const string PingRequest = "Ping";
    public const string ACK = "ACK";
    public const string NAK = "NAK";

    // ------------------------------------------------------------------------
    // Default headers
    // ------------------------------------------------------------------------
    public const string ClientNameHeader = "ClientName"; 
    public const string ClientSecretHeader = "ClientSecret";
    public const string RequestIdHeader = "RequestId";
    public const string ErrorHeader = "Error";
    public const string ErrorDescriptionHeader = "ErrorDescr";

}