using HBLibrary.Interface.IO;
using HBLibrary.Interface.IO.Json;
using HBLibrary.Interface.IO.Storage.Builder;
using HBLibrary.Interface.IO.Xml;
using HBLibrary.IO.Json;
using HBLibrary.IO.Xml;

namespace HBLibrary.IO.Storage.Builder;
public class FileServiceContainer : IFileServiceContainer {
    public IFileService? FileService { get; private set; }
    public IJsonFileService? JsonFileService { get; private set; }
    public IXmlFileService? XmlFileService { get; private set; }

    public IFileServiceContainer UseFileService(Func<IFileService> createFileService) {
        FileService = createFileService();
        return this;
    }

    public IFileServiceContainer UseJsonFileService(Func<IJsonFileService> createJsonFileService) {
        JsonFileService = createJsonFileService();
        return this;
    }

    public IFileServiceContainer UseXmlFileService(Func<IXmlFileService> createXmlFileService) {
        XmlFileService = createXmlFileService();
        return this;
    }
}

public static class FileServiceContainerExtensions {
    public static IFileServiceContainer UseFileService(this IFileServiceContainer container) {
        return container.UseFileService(() => new FileService());
    }
    
    public static IFileServiceContainer UseJsonFileService(this IFileServiceContainer container) {
        return container.UseJsonFileService(() => new JsonFileService());
    }
    
    public static IFileServiceContainer UseXmlFileService(this IFileServiceContainer container) {
        return container.UseXmlFileService(() => new XmlFileService());
    }

    public static IFileServiceContainer UseFileService(this IFileServiceContainer container, Action<FileService> updateFileService) {
        return container.UseFileService(() => {
            FileService fs = new FileService();
            updateFileService(fs);
            return fs;
        });
    }

    public static IFileServiceContainer UseJsonFileService(this IFileServiceContainer container, Action<JsonFileService> updateJsonFileService) {
        return container.UseJsonFileService(() => {
            JsonFileService jfs = new JsonFileService();
            updateJsonFileService(jfs);
            return jfs;
        });
    }

    public static IFileServiceContainer UseXmlFileService(this IFileServiceContainer container, Action<XmlFileService> updateXmlFileService) {
        return container.UseXmlFileService(() => {
            XmlFileService xfs = new XmlFileService();
            updateXmlFileService(xfs);
            return xfs;
        });
    }
}
