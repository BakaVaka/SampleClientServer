using Microsoft.Extensions.Logging;

namespace bProxyTunnelHostServer;

public class DiagnosticObserver : IObserver<KeyValuePair<string, object?>> {
    public void OnCompleted() {

    }
    public void OnError(Exception error) {

    }

    public void OnNext(KeyValuePair<String, Object> value) {
        Program.Logger.LogDebug("{@param}, {@value}", value.Key, value.Value);
    }
}