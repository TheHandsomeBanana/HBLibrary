using System.Text.Json;

namespace HBLibrary.Services.IO.Json;
public class JsonFileService : IJsonFileService {
    private JsonSerializerOptions? options = null;
    private readonly FileService fileService = new FileService();

    public void SetGlobalOptions(JsonSerializerOptions serializerOptions) {
        options = serializerOptions;
    }

    public TJson? ReadJson<TJson>(FileSnapshot file, FileShare share = FileShare.None, JsonSerializerOptions? serializerOptions = null) {
        string content = fileService.Read(file, share);
        return JsonSerializer.Deserialize<TJson?>(content, serializerOptions ?? options);
    }

    public async Task<TJson?> ReadJsonAsync<TJson>(FileSnapshot file, FileShare share = FileShare.None, JsonSerializerOptions? serializerOptions = null) {
        string content = await fileService.ReadAsync(file, share);
        return JsonSerializer.Deserialize<TJson?>(content, serializerOptions ?? options);
    }

    public void WriteJson<TJson>(FileSnapshot file, TJson jsonObject, bool append = false, FileShare share = FileShare.None, JsonSerializerOptions? serializerOptions = null) {
        string content = JsonSerializer.Serialize(jsonObject, serializerOptions ?? options);
        fileService.Write(file, content, append, share);
    }

    public Task WriteJsonAsync<TJson>(FileSnapshot file, TJson jsonObject, bool append = false, FileShare share = FileShare.None, JsonSerializerOptions? serializerOptions = null) {
        string content = JsonSerializer.Serialize(jsonObject, serializerOptions ?? options);
        return fileService.WriteAsync(file, content, append, share);
    }
}
