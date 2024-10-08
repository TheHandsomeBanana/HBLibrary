﻿namespace HBLibrary.Services.IO.Storage.Settings;
public class StorageEntrySettings {
    public EntryLifetime? LifeTime { get; set; }
    public bool EncryptionEnabled { get; set; }

    public static StorageEntrySettings CreateDefault() {
        return new StorageEntrySettings {
            LifeTime = EntryLifetime.CreateSingletonLifetime(),
        };
    }

}