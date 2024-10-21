using HBLibrary.Core.Extensions;
using HBLibrary.Interface.IO;
using HBLibrary.IO.Json;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace HBLibrary.IO.Storage.Config;
public class ApplicationStorageConfig {
    [JsonIgnore]
    private const string NAME = "appstorage.config";
    [JsonIgnore]
    private string basePath = "";

    public Dictionary<Guid, Container> Containers { get; set; } = [];

    private ApplicationStorageConfig(string directory) {
        basePath = directory;
    }

    public void Save() {
        JsonFileService jsonFileService = new JsonFileService();
        string filename = Path.Combine(basePath, NAME);

        jsonFileService.WriteJson(FileSnapshot.Create(filename, true), this, new JsonSerializerOptions { WriteIndented = true });
    }

    public static ApplicationStorageConfig CreateNew(string basePath) {
        ApplicationStorageConfig config = new ApplicationStorageConfig(basePath);

        config.Containers.Add(basePath.ToGuid(), new Container(basePath));
        config.Save();
        return config;
    }

    public static ApplicationStorageConfig? GetConfig(string directory) {
        JsonFileService jsonFileService = new JsonFileService();
        string filename = Path.Combine(directory, NAME);

        if (!FileSnapshot.TryCreate(filename, out FileSnapshot? file, true)) {
            return null;
        }

        ApplicationStorageConfig? config = jsonFileService.ReadJson<ApplicationStorageConfig>(file!);
        if (config is not null) {
            config.basePath = directory;
        }

        return config;
    }
}

public class Container {
    public string Path { get; }

    public Container(string path) {
        Path = path;
    }
}
