using HBLibrary.Common;
using HBLibrary.Common.Extensions;
using HBLibrary.Common.Security;
using System.Runtime.Serialization;
using System.Text;
using System.Text.Json;
using System.Xml.Linq;
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
        using TextWriter sw = new StreamWriter(file.OpenStream(append ? FileMode.Append : FileMode.Create, FileAccess.Write, share));
        serializer.Serialize(sw, xmlObject);
    }

    public void WriteXml(Type type, FileSnapshot file, object xmlObject, bool append = false, FileShare share = FileShare.None) {
        XmlSerializer serializer = new XmlSerializer(type);
        using TextWriter sw = new StreamWriter(file.OpenStream(append ? FileMode.Append : FileMode.Create, FileAccess.Write, share));
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

    public object? DecryptXml(Type type, FileSnapshot file, ICryptographer cryptographer, CryptographyInput input, JsonSerializerOptions? serializerOptions = null, FileShare share = FileShare.None) {
        XmlSerializer serializer = new XmlSerializer(type);
        using FileStream stream = file.OpenStream(FileMode.Open, FileAccess.Read, share);

        byte[] buffer = stream.Read();
        byte[] decrypted = cryptographer.Decrypt(buffer, input);

        using StringReader stringReader = new StringReader(GlobalEnvironment.Encoding.GetString(decrypted));

        return serializer.Deserialize(stringReader);
    }

    public void EncryptXml(Type type, FileSnapshot file, object xmlObject, ICryptographer cryptographer, CryptographyInput input, JsonSerializerOptions? serializerOptions = null, FileShare share = FileShare.None) {
        XmlSerializer serializer = new XmlSerializer(type);
        using StringWriter sw = new StringWriter();
        serializer.Serialize(sw, xmlObject);
        string xmlString = sw.ToString();

        byte[] encrypted = cryptographer.Encrypt(GlobalEnvironment.Encoding.GetBytes(xmlString), input);

        using FileStream stream = file.OpenStream(FileMode.Create, FileAccess.Write, share);
        stream.Write(encrypted);
    }
    
    public TXml? DecryptXml<TXml>(FileSnapshot file, ICryptographer cryptographer, CryptographyInput input, JsonSerializerOptions? serializerOptions = null, FileShare share = FileShare.None) {
        XmlSerializer serializer = new XmlSerializer(typeof(TXml));
        using FileStream stream = file.OpenStream(FileMode.Open, FileAccess.Read, share);

        byte[] buffer = stream.Read();
        byte[] decrypted = cryptographer.Decrypt(buffer, input);

        using StringReader stringReader = new StringReader(GlobalEnvironment.Encoding.GetString(decrypted));

        return (TXml?)serializer.Deserialize(stringReader);
    }

    public void EncryptXml<TXml>(FileSnapshot file, TXml xmlObject, ICryptographer cryptographer, CryptographyInput input, JsonSerializerOptions? serializerOptions = null, FileShare share = FileShare.None) {
        XmlSerializer serializer = new XmlSerializer(typeof(TXml));
        using StringWriter sw = new StringWriter();
        serializer.Serialize(sw, xmlObject);
        string xmlString = sw.ToString();

        byte[] encrypted = cryptographer.Encrypt(GlobalEnvironment.Encoding.GetBytes(xmlString), input);

        using FileStream stream = file.OpenStream(FileMode.Create, FileAccess.Write, share);
        stream.Write(encrypted);
    }

}
