using HBLibrary.Core.Json;
using HBLibrary.Interface.Logging.Configuration;
using System.Globalization;
using System.Runtime.Serialization;
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

    public static LogStatement CreateInfo(string message, string name = "") {
        return new LogStatement(message, name, LogLevel.Info, DateTime.UtcNow);
    }
    
    public static LogStatement CreateWarning(string message, string name = "") {
        return new LogStatement(message, name, LogLevel.Warning, DateTime.UtcNow);
    }
    
    public static LogStatement CreateError(string message, string name = "") {
        return new LogStatement(message, name, LogLevel.Error, DateTime.UtcNow);
    }
    
    public static LogStatement CreateFatal(string message, string name = "") {
        return new LogStatement(message, name, LogLevel.Fatal, DateTime.UtcNow);
    }

    public override readonly string ToString() => Message;
    public readonly string ToFullString()
        => $"Name: {Name}\nCreated On: {CreatedOn:yyyy-MM-dd hh:MM:ss}\nLog Level: {Level}\nMessage: {Message}";
    public readonly string ToDefaultString() => $"[{CreatedOn:hh:MM:ss}] [{Level}]: {Message}";
    public readonly string ToLevelMessage() => $"[{Level}]: {Message}";
    public readonly string ToJson() => JsonSerializer.Serialize(this);
    public readonly string ToXml() {
        using (TextWriter stringwriter = new StringWriter()) {
            XmlSerializer serializer = new XmlSerializer(GetType());
            serializer.Serialize(stringwriter, this);
            return stringwriter.ToString()!;
        }
    }
}