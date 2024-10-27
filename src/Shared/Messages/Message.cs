using System.Xml.Serialization;

namespace Shared.Messages;

[XmlInclude(typeof(MathOperationRequestMessage))]

[XmlInclude(typeof(MathOperationResponseMessage))]
public abstract class Message { }

[XmlRoot("MathOperationRequest")]
public class MathOperationRequestMessage : Message {
    public string Method { get; set; } = "";
    public double[] Operands { get; set; } =  Array.Empty<double>();
}

[XmlRoot("MathOperationResponse")]
public class MathOperationResponseMessage : Message {
    public string Status { get; set; } = "";
    public double Result { get; set; }
}
[XmlRoot("NotSupportedOperation")]
public class NotSupportedOperationMessage : Message { }