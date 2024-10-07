using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace HBLibrary.Services.IO.Xml;
public class XmlFileService : IXmlFileService {
    public TXml? ReadXml<TXml>(FileSnapshot file, FileShare share = FileShare.None) {
        XmlSerializer serializer = new XmlSerializer(typeof(TXml));
        return (TXml?)serializer.Deserialize(file.OpenStream(FileMode.Open, FileAccess.Read, share));
    }

    public object? ReadXml(Type type, FileSnapshot file, FileShare share = FileShare.None) {
        XmlSerializer serializer = new XmlSerializer(type);
        return serializer.Deserialize(file.OpenStream(FileMode.Open, FileAccess.Read, share));
    }

    public async Task<object?> ReadXmlAsync(Type type, FileSnapshot file, FileShare share = FileShare.None) {
        XmlSerializer serializer = new XmlSerializer(type);
        using FileStream stream = file.OpenStream(FileMode.Open, FileAccess.Read, share);
        using StreamReader reader = new StreamReader(stream);

        string xmlContent = await reader.ReadToEndAsync();
        using StringReader stringReader = new StringReader(xmlContent);

        return serializer.Deserialize(stringReader);
    }

    public async Task<TXml?> ReadXmlAsync<TXml>(FileSnapshot file, FileShare share = FileShare.None) {
        XmlSerializer serializer = new XmlSerializer(typeof(TXml));
        using FileStream stream = file.OpenStream(FileMode.Open, FileAccess.Read, share);
        using StreamReader reader = new StreamReader(stream);

        string xmlContent = await reader.ReadToEndAsync();
        using StringReader stringReader = new StringReader(xmlContent);

        return (TXml?)serializer.Deserialize(stringReader);
    }

    public void WriteXml<TXml>(FileSnapshot file, TXml xmlObject, bool append = false, FileShare share = FileShare.None) {
        XmlSerializer serializer = new XmlSerializer(typeof(TXml));
        using TextWriter sw = new StreamWriter(file.OpenStream(append ? FileMode.Append : FileMode.Open, FileAccess.Write, share));
        serializer.Serialize(sw, xmlObject);
    }

    public void WriteXml(Type type, FileSnapshot file, object xmlObject, bool append = false, FileShare share = FileShare.None) {
        XmlSerializer serializer = new XmlSerializer(type);
        using TextWriter sw = new StreamWriter(file.OpenStream(append ? FileMode.Append : FileMode.Open, FileAccess.Write, share));
        serializer.Serialize(sw, xmlObject);
    }

    public Task WriteXmlAsync(Type type, FileSnapshot file, object xmlObject, bool append = false, FileShare share = FileShare.None) {
        XmlSerializer serializer = new XmlSerializer(type);
        using FileStream stream = file.OpenStream(append ? FileMode.Append : FileMode.Create, FileAccess.Write, share);
        using StreamWriter writer = new StreamWriter(stream);

        using StringWriter stringWriter = new StringWriter();
        serializer.Serialize(stringWriter, xmlObject);

        return writer.WriteAsync(stringWriter.ToString());
    }

    public Task WriteXmlAsync<TXml>(FileSnapshot file, TXml xmlObject, bool append = false, FileShare share = FileShare.None) {
        XmlSerializer serializer = new XmlSerializer(typeof(TXml));
        using FileStream stream = file.OpenStream(append ? FileMode.Append : FileMode.Create, FileAccess.Write, share);
        using StreamWriter writer = new StreamWriter(stream);

        using StringWriter stringWriter = new StringWriter();
        serializer.Serialize(stringWriter, xmlObject);

        return writer.WriteAsync(stringWriter.ToString());
    }
}
