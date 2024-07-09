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
}
