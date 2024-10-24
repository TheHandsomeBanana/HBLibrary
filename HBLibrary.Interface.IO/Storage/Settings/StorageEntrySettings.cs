using System.Text.Json.Serialization;

namespace HBLibrary.Interface.IO.Storage.Settings;
public class StorageEntrySettings {
    public EntryLifetime? LifeTime { get; set; }
    public bool EncryptionEnabled { get; set; }
    public bool UseTrackingHistory { get; set; }

    [JsonIgnore]
    public StorageContainerCryptography? ContainerCryptography { get; set; }

    public static StorageEntrySettings CreateDefault() {
        return new StorageEntrySettings {
            LifeTime = EntryLifetime.CreateSingletonLifetime(),
        };
    }

}