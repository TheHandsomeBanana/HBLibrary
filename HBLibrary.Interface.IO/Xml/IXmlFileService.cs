using HBLibrary.Interface.Security;
using System.Text;
using System.Text.Json;

namespace HBLibrary.Interface.IO.Xml;
public interface IXmlFileService {
    object? ReadXml(Type type, FileSnapshot file, FileShare share = FileShare.None);
    TXml? ReadXml<TXml>(FileSnapshot file, FileShare share = FileShare.None);
    void WriteXml(Type type, FileSnapshot file, object xmlObject, bool append = false, FileShare share = FileShare.None);
    void WriteXml<TXml>(FileSnapshot file, TXml xmlObject, bool append = false, FileShare share = FileShare.None);

    Task<object?> ReadXmlAsync(Type type, FileSnapshot file, FileShare share = FileShare.None);
    Task<TXml?> ReadXmlAsync<TXml>(FileSnapshot file, FileShare share = FileShare.None);
    Task WriteXmlAsync(Type type, FileSnapshot file, object xmlObject, bool append = false, FileShare share = FileShare.None);
    Task WriteXmlAsync<TXml>(FileSnapshot file, TXml xmlObject, bool append = false, FileShare share = FileShare.None);

    object? DecryptXml(Type type, FileSnapshot file, ICryptographer cryptographer, CryptographyInput input, JsonSerializerOptions? serializerOptions = null, FileShare share = FileShare.None);
    void EncryptXml(Type type, FileSnapshot file, object xmlObject, ICryptographer cryptographer, CryptographyInput input, JsonSerializerOptions? serializerOptions = null, FileShare share = FileShare.None);
    TXml? DecryptXml<TXml>(FileSnapshot file, ICryptographer cryptographer, CryptographyInput input, JsonSerializerOptions? serializerOptions = null, FileShare share = FileShare.None);
    void EncryptXml<TXml>(FileSnapshot file, TXml xmlObject, ICryptographer cryptographer, CryptographyInput input, JsonSerializerOptions? serializerOptions = null, FileShare share = FileShare.None);
}
