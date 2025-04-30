using Shared;

namespace Server;

public class TcpServerSettings {
    public IReadOnlyCollection<UInt16> ListenPorts { get; }
    public ConnectionHandlerDelegate Application { get; }
    public TcpServerSettings(ConnectionHandlerDelegate application, params UInt16[] listen) {
        ArgumentNullException.ThrowIfNull(application, nameof(application));
        ArgumentNullException.ThrowIfNull(listen, nameof(listen));
        Application = application;
        ListenPorts = new HashSet<ushort>(listen);
    }
}