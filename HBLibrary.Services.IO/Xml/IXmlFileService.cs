namespace HBLibrary.Services.IO.Xml;
public interface IXmlFileService {
    TXml? ReadXml<TXml>(FileSnapshot file, FileShare share = FileShare.None);
    void WriteXml<TXml>(FileSnapshot file, TXml xmlObject, bool append = false, FileShare share = FileShare.None);
}
