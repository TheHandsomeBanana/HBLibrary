namespace HBLibrary.Services.IO.Xml;
public interface IXmlFileService {
    object? ReadXml(Type type, FileSnapshot file, FileShare share = FileShare.None);
    TXml? ReadXml<TXml>(FileSnapshot file, FileShare share = FileShare.None);
    void WriteXml(Type type, FileSnapshot file, object xmlObject, bool append = false, FileShare share = FileShare.None);
    void WriteXml<TXml>(FileSnapshot file, TXml xmlObject, bool append = false, FileShare share = FileShare.None);

    Task<object?> ReadXmlAsync(Type type, FileSnapshot file, FileShare share = FileShare.None);
    Task<TXml?> ReadXmlAsync<TXml>(FileSnapshot file, FileShare share = FileShare.None);
    Task WriteXmlAsync(Type type, FileSnapshot file, object xmlObject, bool append = false, FileShare share = FileShare.None);
    Task WriteXmlAsync<TXml>(FileSnapshot file, TXml xmlObject, bool append = false, FileShare share = FileShare.None);

    
}
