using HBLibrary.Interface.IO.Json;
using HBLibrary.Interface.IO.Xml;

namespace HBLibrary.Interface.IO.Storage.Builder;
public interface IFileServiceContainer
{
    public IFileService? FileService { get; }
    public IJsonFileService? JsonFileService { get; }
    public IXmlFileService? XmlFileService { get; }

    public IFileServiceContainer UseFileService(Func<IFileService> createFileService);
    public IFileServiceContainer UseJsonFileService(Func<IJsonFileService> createJsonFileService);
    public IFileServiceContainer UseXmlFileService(Func<IXmlFileService> createXmlFileService);
}
