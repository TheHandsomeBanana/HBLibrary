using System.Text.Json;

namespace HBLibrary.Services.IO.Json;
public interface IJsonFileService {
    void SetGlobalOptions(JsonSerializerOptions serializerOptions);
    bool UseBase64 { get; }
    object? ReadJson(Type type, FileSnapshot file, JsonSerializerOptions? serializerOptions = null, FileShare share = FileShare.None);
    TJson? ReadJson<TJson>(FileSnapshot file, JsonSerializerOptions? serializerOptions = null, FileShare share = FileShare.None);
    Task<object?> ReadJsonAsync(Type type, FileSnapshot file, JsonSerializerOptions? serializerOptions = null, FileShare share = FileShare.None);
    Task<TJson?> ReadJsonAsync<TJson>(FileSnapshot file, JsonSerializerOptions? serializerOptions = null, FileShare share = FileShare.None);
    void WriteJson(Type type, FileSnapshot file, object jsonObject, JsonSerializerOptions? serializerOptions = null, bool append = false, FileShare share = FileShare.None);
    void WriteJson<TJson>(FileSnapshot file, TJson jsonObject, JsonSerializerOptions? serializerOptions = null, bool append = false, FileShare share = FileShare.None);
    Task WriteJsonAsync(Type type, FileSnapshot file, object jsonObject, JsonSerializerOptions? serializerOptions = null, bool append = false, FileShare share = FileShare.None);
    Task WriteJsonAsync<TJson>(FileSnapshot file, TJson jsonObject, JsonSerializerOptions? serializerOptions = null, bool append = false, FileShare share = FileShare.None);
}
