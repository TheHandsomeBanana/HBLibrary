using HBLibrary.Common;
using HBLibrary.Common.Security;
using System.Text.Json;

namespace HBLibrary.Services.IO.Json;
public class JsonFileService : IJsonFileService {
    private JsonSerializerOptions? options = null;
    private readonly FileService fileService = new FileService();

    public JsonFileService() { }

    public bool UseBase64 { get; set; }

    public void SetGlobalOptions(JsonSerializerOptions serializerOptions) {
        options = serializerOptions;
    }

    public object? ReadJson(Type type, FileSnapshot file, JsonSerializerOptions? serializerOptions = null, FileShare share = FileShare.None) {
        string content = fileService.Read(file, share);

        if (UseBase64) {
            content = GlobalEnvironment.Encoding.GetString(Convert.FromBase64String(content));
        }

        try {
            return JsonSerializer.Deserialize(content, type, serializerOptions ?? options);
        }
        catch (JsonException) {
            return null;
        }
    }

    public TJson? ReadJson<TJson>(FileSnapshot file, JsonSerializerOptions? serializerOptions = null, FileShare share = FileShare.None) {
        string content = fileService.Read(file, share);

        if (UseBase64) {
            content = GlobalEnvironment.Encoding.GetString(Convert.FromBase64String(content));
        }

        try {
            return JsonSerializer.Deserialize<TJson?>(content, serializerOptions ?? options);
        }
        catch (JsonException) {
            return default;
        }
    }

    public async Task<object?> ReadJsonAsync(Type type, FileSnapshot file, JsonSerializerOptions? serializerOptions = null, FileShare share = FileShare.None) {
        string content = await fileService.ReadAsync(file, share);

        if (UseBase64) {
            content = GlobalEnvironment.Encoding.GetString(Convert.FromBase64String(content));
        }

        try {
            return JsonSerializer.Deserialize(content, type, serializerOptions ?? options);
        }
        catch (JsonException) {
            return null;
        }
    }

    public async Task<TJson?> ReadJsonAsync<TJson>(FileSnapshot file, JsonSerializerOptions? serializerOptions = null, FileShare share = FileShare.None) {
        string content = await fileService.ReadAsync(file, share);

        if (UseBase64) {
            content = GlobalEnvironment.Encoding.GetString(Convert.FromBase64String(content));
        }

        try {
            return JsonSerializer.Deserialize<TJson?>(content, serializerOptions ?? options);
        }
        catch (JsonException) {
            return default;
        }
    }

    public void WriteJson(Type type, FileSnapshot file, object jsonObject, JsonSerializerOptions? serializerOptions = null, bool append = false, FileShare share = FileShare.None) {
        string content = JsonSerializer.Serialize(jsonObject, type, serializerOptions ?? options);

        if (UseBase64) {
            content = Convert.ToBase64String(GlobalEnvironment.Encoding.GetBytes(content));
        }

        fileService.Write(file, content, append, share);
    }

    public void WriteJson<TJson>(FileSnapshot file, TJson jsonObject, JsonSerializerOptions? serializerOptions = null, bool append = false, FileShare share = FileShare.None) {
        string content = JsonSerializer.Serialize(jsonObject, serializerOptions ?? options);

        if (UseBase64) {
            content = Convert.ToBase64String(GlobalEnvironment.Encoding.GetBytes(content));
        }

        fileService.Write(file, content, append, share);
    }

    public Task WriteJsonAsync(Type type, FileSnapshot file, object jsonObject, JsonSerializerOptions? serializerOptions = null, bool append = false, FileShare share = FileShare.None) {
        string content = JsonSerializer.Serialize(jsonObject, type, serializerOptions ?? options);

        if (UseBase64) {
            content = Convert.ToBase64String(GlobalEnvironment.Encoding.GetBytes(content));
        }

        return fileService.WriteAsync(file, content, append, share);
    }

    public Task WriteJsonAsync<TJson>(FileSnapshot file, TJson jsonObject, JsonSerializerOptions? serializerOptions = null, bool append = false, FileShare share = FileShare.None) {
        string content = JsonSerializer.Serialize(jsonObject, serializerOptions ?? options);

        if (UseBase64) {
            content = Convert.ToBase64String(GlobalEnvironment.Encoding.GetBytes(content));
        }

        return fileService.WriteAsync(file, content, append, share);
    }

    public object? DecryptJson(Type type, FileSnapshot file, ICryptographer cryptographer, CryptographyInput input, JsonSerializerOptions? serializerOptions = null, FileShare share = FileShare.None) {
        string content = fileService.Decrypt(file, cryptographer, input, GlobalEnvironment.Encoding, share);

        if (UseBase64) {
            content = GlobalEnvironment.Encoding.GetString(Convert.FromBase64String(content));
        }

        try {
            return JsonSerializer.Deserialize(content, type, serializerOptions ?? options);
        }
        catch (JsonException) {
            return default;
        }
    }

    public void EncryptJson(Type type, FileSnapshot file, object jsonObject, ICryptographer cryptographer, CryptographyInput input, JsonSerializerOptions? serializerOptions = null, FileShare share = FileShare.None) {
        string content = JsonSerializer.Serialize(jsonObject, type, serializerOptions ?? options);

        if (UseBase64) {
            content = Convert.ToBase64String(GlobalEnvironment.Encoding.GetBytes(content));
        }

        fileService.Encrypt(file, content, cryptographer, input, GlobalEnvironment.Encoding, share);
    }
    
    public TJson? DecryptJson<TJson>(FileSnapshot file, ICryptographer cryptographer, CryptographyInput input, JsonSerializerOptions? serializerOptions = null, FileShare share = FileShare.None) {
        string content = fileService.Decrypt(file, cryptographer, input, GlobalEnvironment.Encoding, share);

        if (UseBase64) {
            content = GlobalEnvironment.Encoding.GetString(Convert.FromBase64String(content));
        }

        try {
            return JsonSerializer.Deserialize<TJson?>(content, serializerOptions ?? options);
        }
        catch (JsonException) {
            return default;
        }
    }

    public void EncryptJson<TJson>(FileSnapshot file, TJson jsonObject, ICryptographer cryptographer, CryptographyInput input, JsonSerializerOptions? serializerOptions = null, FileShare share = FileShare.None) {
        string content = JsonSerializer.Serialize(jsonObject, serializerOptions ?? options);

        if (UseBase64) {
            content = Convert.ToBase64String(GlobalEnvironment.Encoding.GetBytes(content));
        }

        fileService.Encrypt(file, content, cryptographer, input, GlobalEnvironment.Encoding, share);
    }
}
