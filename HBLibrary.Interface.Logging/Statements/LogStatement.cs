using HBLibrary.Core.Json;
using HBLibrary.Interface.Logging.Configuration;
using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Xml.Serialization;

namespace HBLibrary.Interface.Logging.Statements;
[Serializable]
public struct LogStatement {
    public string Message { get; set; }
    public string Name { get; set; }
    public LogLevel Level { get; set; }
    [XmlIgnore]
    [JsonDateTimeFormat("yyyy-MM-dd hh:mm:ss")]
    public DateTime CreatedOn { get; set; }

    [XmlElement("CreatedOn")]
    public string CreatedOnFormatted {
        get { return CreatedOn.ToString("yyyy-MM-dd hh:mm:ss", CultureInfo.InvariantCulture); }
        set { CreatedOn = DateTime.ParseExact(value, "yyyy-MM-dd hh:mm:ss", CultureInfo.InvariantCulture); }
    }

    [JsonConstructor]
    public LogStatement(string message, string name, LogLevel level, DateTime createdOn) {
        Message = message;
        Name = name;
        Level = level;
        CreatedOn = createdOn;
    }

    public override readonly string ToString() => $"[{Level}]: {Message}";
    public readonly string ToFullString()
        => $"Name: {Name}\nCreated On: {CreatedOn:yyyy-MM-dd hh:MM:ss}\nLog Level: {Level}\nMessage: {Message}";
    public readonly string ToMinimalString() => $"[{CreatedOn:hh:MM:ss}] [{Level}]: {Message}";
    public readonly string ToJson() => JsonSerializer.Serialize(this);
    public readonly string ToXml() {
        using (TextWriter stringwriter = new StringWriter()) {
            var serializer = new XmlSerializer(GetType());
            serializer.Serialize(stringwriter, this);
            return stringwriter.ToString()!;
        }
    }

    public readonly string Format(LogDisplayFormat format) {
        switch (format) {
            case LogDisplayFormat.MessageOnly:
                return ToString();
            case LogDisplayFormat.Minimal:
                return ToMinimalString();
            case LogDisplayFormat.Full:
                return ToFullString();
            default:
                return ToString();
        }
    }
}