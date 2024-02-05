using HBLibrary.Common.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HBLibrary.Services.IO;
public class FileService : IFileService {
    public string Read(FileSnapshot file) {
        throw new NotImplementedException();
    }

    public Task<string> ReadAsync(FileSnapshot file) {
        throw new NotImplementedException();
    }

    public TJson ReadFromJson<TJson>(FileSnapshot file) {
        throw new NotImplementedException();
    }

    public Task<TJson> ReadFromJsonAsync<TJson>(FileSnapshot file) {
        throw new NotImplementedException();
    }

    public TXml ReadFromXml<TXml>(FileSnapshot file) {
        throw new NotImplementedException();
    }

    public Task<TXml> ReadFromXmlAsync<TXml>(FileSnapshot file) {
        throw new NotImplementedException();
    }

    public void Write(FileSnapshot file, string content) {
        throw new NotImplementedException();
    }

    public Task WriteAsync(FileSnapshot file, string content) {
        throw new NotImplementedException();
    }

    public void WriteJson<TJson>(FileSnapshot file, TJson json) {
        throw new NotImplementedException();
    }

    public Task WriteJsonAsync<TJson>(FileSnapshot file, TJson json) {
        throw new NotImplementedException();
    }

    public void WriteXml<TXml>(FileSnapshot file, TXml xml) {
        throw new NotImplementedException();
    }

    public Task WriteXmlAsync<TXml>(FileSnapshot file, TXml xml) {
        throw new NotImplementedException();
    }
}
