using HBLibrary.Common.Security;
using HBLibrary.Common.Security.Keys;
using System.Text.Json.Serialization;

namespace HBLibrary.Services.IO.Storage.Settings;
public class StorageEntrySettings {
    public EntryLifetime? LifeTime { get; set; }
    public bool EncryptionEnabled { get; set; }

    [JsonIgnore]
    public StorageContainerCryptography? ContainerCryptography { get; set; }

    public static StorageEntrySettings CreateDefault() {
        return new StorageEntrySettings {
            LifeTime = EntryLifetime.CreateSingletonLifetime(),
        };
    }

}