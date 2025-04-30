using System.Xml.Serialization;

namespace bProxyTunnelCore.Messages;

[XmlInclude(typeof(Request))]
[XmlInclude(typeof(Response))]
[XmlInclude(typeof(Ping))]
[XmlInclude(typeof(Connect))]
[XmlInclude(typeof(Disconnect))]
[XmlInclude(typeof(DataTransfer))]
public abstract class Message { }

[XmlRoot("Ping")]
public class Ping : Message { }

[XmlRoot("Connect")]
public class Connect : Message {
    public string ClientId { get; set; }
    public string IP { get; set; }
    public int Port { get; set; }
}

[XmlRoot("Disconnect")]
public class Disconnect : Message {
    public string ClientId { get; set; }
    public string IP {  get; set; }
    public int Port { get; set; }

    /// <summary>
    /// Причина отключения(сам или из-за какой-то ошибки)
    /// </summary>
    public string? Reason { get; set; }
}

[XmlRoot("DataTransfer")]
public class DataTransfer {
    public byte[]? Payload { get; set; }
}

[XmlRoot("Request")]
public class Request : Message {
    public Meta Meta { get; set; }

    [XmlArray("Headers")]
    [XmlArrayItem("Item")]
    public HeaderData[] Headers { get; set; }

    public Payload? Payload { get; set; } = null;
}

[XmlRoot("Response")]
public class Response : Message {
    public Meta Meta { get; set; }

    [XmlArray("Headers")]
    [XmlArrayItem("Item")]
    public HeaderData[] Headers { get; set; }
    public Payload? Payload { get; set; } = null;
}

public class HeaderData {

    public HeaderData() : this("", "") { }
    public HeaderData(String type, String value) {
        Type = type;
        Value = value;
    }

    [XmlAttribute("Type")]
    public string Type { get; set; } = "";

    [XmlAttribute("Value")]
    public string Value { get; set; } = "";
}

public class Meta {
    public string Type { get; set; }
    public long MessageId { get; set; }
    public long Timestamp { get; set; } = DateTime.UtcNow.Ticks;
}

public class Payload {
    public string Type { get; set; }
    public byte[]? Body { get; set; } = null;
}

