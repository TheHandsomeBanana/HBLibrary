using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace HBLibrary.Services.IO.Json;
public interface IJsonFileService {
    void SetGlobalOptions(JsonSerializerOptions serializerOptions);
    TJson? ReadJson<TJson>(FileSnapshot file, FileShare share = FileShare.None, JsonSerializerOptions? serializerOptions = null);
    Task<TJson?> ReadJsonAsync<TJson>(FileSnapshot file, FileShare share = FileShare.None, JsonSerializerOptions? serializerOptions = null);
    void WriteJson<TJson>(FileSnapshot file, TJson jsonObject, bool append = false, FileShare share = FileShare.None, JsonSerializerOptions? serializerOptions = null);
    Task WriteJsonAsync<TJson>(FileSnapshot file, TJson jsonObject, bool append = false, FileShare share = FileShare.None, JsonSerializerOptions? serializerOptions = null);
}
