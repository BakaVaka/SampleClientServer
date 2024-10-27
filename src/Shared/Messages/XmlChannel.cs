using System.Text;
using System.Xml.Serialization;

namespace Shared.Messages;
public static class XmlChannel
{
    private static readonly Type[] _serializerTypes =  new[] { 
        typeof(MathOperationRequestMessage), 
        typeof(MathOperationResponseMessage),
        typeof(NotSupportedOperationMessage)
    };

    private static readonly XmlSerializer _serializer = CreateSerializer();

    private static XmlSerializer CreateSerializer() {
        return new XmlSerializer(typeof(Message), _serializerTypes);
    }

    public static async Task<Message?> Decode(Stream stream, CancellationToken cancellationToken = default) {
        StringBuilder sb = new();
        byte[] buffer = new byte[1024 * 1024];
        do {
            var readed = await stream.ReadAsync(buffer, cancellationToken);
            // disconencted
            if(readed == 0 ) {
                return null;
            }
            sb.Append(Encoding.UTF8.GetString(buffer.AsMemory(0, readed).Span));
            break;
        } while( true );

        MemoryStream ms = new();
        await ms.WriteAsync(Encoding.UTF8.GetBytes(sb.ToString()));   
        ms.Position = 0;
        var result = _serializer.Deserialize(ms);
        return result as Message;
    }

    public static ReadOnlyMemory<byte> Encode(Message message) {
        using var ms = new MemoryStream();
        _serializer.Serialize(ms, message);
        return ms.ToArray();
    }

}
