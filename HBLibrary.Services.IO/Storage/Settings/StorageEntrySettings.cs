using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace HBLibrary.Services.IO.Storage.Settings;
public class StorageEntrySettings {
    public EntryLifetime LifeTime { get; set; }
    public bool EncryptionEnabled { get; set; }

    public static StorageEntrySettings CreateDefault() {
        return new StorageEntrySettings {
            LifeTime = EntryLifetime.CreateSingletonLifetime(),
        };
    }

}