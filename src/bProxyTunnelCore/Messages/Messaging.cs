namespace bProxyTunnelCore.Messages;
public static class Messaging {

    private static long MessageCounter = 1;

    public static Message CreateHelloRequest(string clientName, string clientSecret, Dictionary<string, string>? additionalHeaders = null) {
        var msgId = Interlocked.Increment(ref MessageCounter);
        return new Request {
            Meta = new Meta {
                Type = Consts.HelloRequest,
                MessageId = msgId,
            },
            Headers =
            [
                new HeaderData(nameof(clientName), clientName),
                new HeaderData(nameof(clientSecret), clientSecret),
                ..(additionalHeaders ?? []).Select(x=> new HeaderData(x.Key, x.Value))
            ]
        };
    }

    public static Response CreateACK(Request request) {
        var msgId = Interlocked.Increment(ref MessageCounter);
        return new Response {
            Meta = new Meta {
                Type = Consts.ACK,
                MessageId = msgId,
            },
            Headers =
            [
                new HeaderData(Consts.RequestIdHeader, request.Meta.MessageId.ToString()),
            ],
        };
    }

    public static Response CreateNAK(Request request, int errorCode, string description) {
        var msgId = Interlocked.Increment(ref MessageCounter);
        return new Response {
            Meta = new Meta {
                Type = Consts.NAK,
                MessageId = msgId,
            },
            Headers =
            [
                new HeaderData(Consts.RequestIdHeader, request.Meta.MessageId.ToString()),
                new HeaderData(Consts.ErrorHeader, errorCode.ToString()),
                new HeaderData(Consts.ErrorDescriptionHeader, description),
            ],
        };
    }
}
