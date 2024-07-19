using HBLibrary.Services.IO.Json;
using HBLibrary.Services.IO.Xml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HBLibrary.Services.IO.Storage.Builder;
public class FileServiceContainer {
    internal IFileService? FileService { get; private set; }
    internal IJsonFileService? JsonFileService { get; private set; }
    internal IXmlFileService? XmlFileService { get; private set; }

    public FileServiceContainer UseFileService(Func<IFileService> createFileService) {
        FileService = createFileService();
        return this;
    }

    public FileServiceContainer UseJsonFileService(Func<IJsonFileService> createJsonFileService) {
        JsonFileService = createJsonFileService();
        return this;
    }

    public FileServiceContainer UseXmlFileService(Func<IXmlFileService> createXmlFileService) {
        XmlFileService = createXmlFileService();
        return this;
    }

    public FileServiceContainer UseFileService(Action<FileService> updateFileService) {
        FileService fs = new FileService();
        updateFileService(fs);
        this.FileService = fs;
        return this;
    }

    public FileServiceContainer UseJsonFileService(Action<JsonFileService> updateJsonFileService) {
        JsonFileService jfs = new JsonFileService();
        updateJsonFileService(jfs);
        this.JsonFileService = jfs;
        return this;
    }
    
    public FileServiceContainer UseXmlFileService(Action<XmlFileService> updateXmlFileService) {
        XmlFileService xfs = new XmlFileService();
        updateXmlFileService(xfs);
        this.XmlFileService = xfs;
        return this;
    }
}
