﻿using HBLibrary.Common.Extensions;
using HBLibrary.Services.IO.Json;
using HBLibrary.Services.IO.Storage.Entries;
using HBLibrary.Services.IO.Storage.Settings;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace HBLibrary.Services.IO.Storage.Config;
public class StorageContainerConfig {
    [JsonIgnore]
    private const string EXTENSION = ".config";
    [JsonIgnore]
    private string filename = "";

    [JsonPropertyName(nameof(ContainerId))]
    public Guid ContainerId { get; set; }

    [JsonPropertyName(nameof(Entries))]
    public Dictionary<string, ContainerEntry> Entries { get; set; } = [];

    private StorageContainerConfig(string basePath) {
        Directory.CreateDirectory(basePath);

        this.ContainerId = basePath.ToGuid();
        this.filename = Path.Combine(basePath, ContainerId.ToString() + EXTENSION);
    }

    [JsonConstructor]
    public StorageContainerConfig() { }

    public void Save() {
        JsonFileService jsonFileService = new JsonFileService();
        jsonFileService.WriteJson(FileSnapshot.Create(filename, true), this, new JsonSerializerOptions { WriteIndented = true });
    }

    public Task SaveAsync() {
        JsonFileService jsonFileService = new JsonFileService();
        return jsonFileService.WriteJsonAsync(FileSnapshot.Create(filename, true), this, new JsonSerializerOptions { WriteIndented = true });
    }

    public static StorageContainerConfig CreateNew(string basePath) {
        StorageContainerConfig config = new StorageContainerConfig(basePath);
        config.Save();
        return config;
    }

    public static StorageContainerConfig? GetConfig(string basePath) {
        string filename = Path.Combine(basePath, basePath.ToGuidString() + EXTENSION);
        JsonFileService jsonFileService = new JsonFileService();

        if (!FileSnapshot.TryCreate(filename, out FileSnapshot? file)) {
            return null;
        }

        StorageContainerConfig? config = jsonFileService.ReadJson<StorageContainerConfig>(file!);
        if (config is not null) {
            config.filename = filename;
        }

        return config;
    }
}

public class ContainerEntry {
    public StorageEntryContentType ContentType { get; init; }
    public StorageEntrySettings Settings { get; init; }

    public ContainerEntry(StorageEntryContentType contentType, StorageEntrySettings settings) {
        ContentType = contentType;
        Settings = settings;
    }
}
