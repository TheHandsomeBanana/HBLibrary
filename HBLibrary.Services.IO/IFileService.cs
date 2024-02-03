using HBLibrary.Common.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace HBLibrary.Services.IO;
public interface IFileService {
    string Read(FileSnapshot file);
    Task<string> ReadAsync(FileSnapshot file);
    void Write(FileSnapshot file, string content);
    Task WriteAsync(FileSnapshot file, string content);

    TJson ReadFromJson<TJson>(FileSnapshot file);
    Task<TJson> ReadFromJsonAsync<TJson>(FileSnapshot file);
    void WriteJson<TJson>(FileSnapshot file, TJson json);
    Task WriteJsonAsync<TJson>(FileSnapshot file, TJson json);

    TXml ReadFromXml<TXml>(FileSnapshot file);
    Task<TXml> ReadFromXmlAsync<TXml>(FileSnapshot file);
    void WriteXml<TXml>(FileSnapshot file, TXml xml);
    Task WriteXmlAsync<TXml>(FileSnapshot file, TXml xml);
}
