using HBLibrary.DataStructures;
using HBLibrary.Interface.Core.ChangeTracker;
using HBLibrary.Interface.IO;
using HBLibrary.Interface.IO.Storage.Entries;
using HBLibrary.Interface.IO.Storage.Settings;
using HBLibrary.Interface.IO.Xml;
using HBLibrary.Interface.Security;
using HBLibrary.Interface.Security.Keys;
using HBLibrary.Security;

namespace HBLibrary.IO.Storage.Entries;
public class StorageXmlEntry : StorageEntry, IStorageEntry {
    private readonly IXmlFileService xmlService;
    internal StorageXmlEntry(IXmlFileService xmlService, string filename, StorageEntrySettings settings, IChangeTracker? changeTracker)
        : base(filename, StorageEntryContentType.Xml, settings, changeTracker) {

        this.xmlService = xmlService;
    }

    public object? Get(Type type) {
        lock (Lock) {

            try {
                if (Settings.LifeTime!.Type == EntryLifetimeType.NoLifetime) {
                    // container.AddOrUpdate called and not saved yet
                    // Then Value is set but file is not written yet
                    if (Value is not null) {
                        return Value;
                    }

                    if (!FileSnapshot.TryCreate(Filename, out FileSnapshot? file)) {
                        return default;
                    }

                    return GetInternal(type, file);
                }
                else {
                    if (Value is null) {

                        if (!FileSnapshot.TryCreate(Filename, out FileSnapshot? file)) {
                            return default;
                        }

                        Value = GetInternal(type, file);
                    }

                    return Value;
                }
            }
            catch {
                return default;
            }
        }
    }

    private object? GetInternal(Type type, FileSnapshot file) {
        object? value;
        if (Settings.EncryptionEnabled) {
            Result<IKey> keyResult = Settings.ContainerCryptography!.GetEntryKey.Invoke();

            IKey key = keyResult.GetValueOrThrow();

            value = xmlService.DecryptXml(type, file, new Cryptographer(), new CryptographyInput {
                Key = key,
                Mode = Settings.ContainerCryptography.CryptographyMode
            });
        }
        else {
            value = xmlService.ReadXml(type, file);
        }

        if (value is INotifyTrackableChanged notifyTrackableChanged) {
            ChangeTracker?.Track(notifyTrackableChanged);
            ChangeTracker?.HookStateChanged(notifyTrackableChanged);
        }

        return value;
    }

    public async Task<object?> GetAsync(Type type) {
        await Semaphore.WaitAsync();
        try {
            if (Settings.LifeTime!.Type == EntryLifetimeType.NoLifetime) {
                // container.AddOrUpdate called and not saved yet
                // Then Value is set but file is not written yet
                if (Value is not null) {
                    return Value;
                }

                if (!FileSnapshot.TryCreate(Filename, out FileSnapshot? file)) {
                    return default;
                }

                return await GetAsyncInternal(type, file);
            }
            else {
                if (Value is null) {
                    if (!FileSnapshot.TryCreate(Filename, out FileSnapshot? file)) {
                        return default;
                    }

                    Value = await GetAsyncInternal(type, file);
                }

                return Value;
            }
        }
        catch {
            return default;
        }
        finally {
            Semaphore.Release();
        }
    }

    private async Task<object?> GetAsyncInternal(Type type, FileSnapshot file) {
        object? value;

        if (Settings.EncryptionEnabled) {
            Result<IKey> keyResult = await Settings.ContainerCryptography!.GetEntryKeyAsync.Invoke();

            IKey key = keyResult.GetValueOrThrow();

            value = xmlService.DecryptXml(type, file, new Cryptographer(), new CryptographyInput {
                Key = key,
                Mode = Settings.ContainerCryptography.CryptographyMode
            });
        }
        else {
            value = await xmlService.ReadXmlAsync(type, file);
        }

        if (value is INotifyTrackableChanged notifyTrackableChanged) {
            ChangeTracker?.Track(notifyTrackableChanged);
            ChangeTracker?.HookStateChanged(notifyTrackableChanged);
        }

        return value;
    }

    public void Save() {
        lock (Lock) {
            if (Value is not null) {
                if (Settings.EncryptionEnabled) {
                    Result<IKey> keyResult = Settings.ContainerCryptography!.GetEntryKey.Invoke();

                    IKey key = keyResult.GetValueOrThrow();

                    xmlService.EncryptXml(Value.GetType(), FileSnapshot.Create(Filename, true), Value, new Cryptographer(), new CryptographyInput {
                        Key = key,
                        Mode = Settings.ContainerCryptography.CryptographyMode
                    });
                }
                else {
                    xmlService.WriteXml(Value.GetType(), FileSnapshot.Create(Filename, true), Value);
                }

                if (Value is INotifyTrackableChanged notifyTrackableChanged) {
                    ChangeTracker?.SaveChanges(notifyTrackableChanged);
                }
            }
        }
    }

    public async Task SaveAsync() {
        await Semaphore.WaitAsync();
        try {

            if (Value is not null) {
                if (Settings.EncryptionEnabled) {
                    Result<IKey> keyResult = await Settings.ContainerCryptography!.GetEntryKeyAsync.Invoke();

                    IKey key = keyResult.GetValueOrThrow();

                    xmlService.EncryptXml(Value.GetType(), FileSnapshot.Create(Filename, true), Value, new Cryptographer(), new CryptographyInput {
                        Key = key,
                        Mode = Settings.ContainerCryptography.CryptographyMode
                    });
                }
                else {
                    await xmlService.WriteXmlAsync(Value.GetType(), FileSnapshot.Create(Filename, true), Value);
                }

                if (Value is INotifyTrackableChanged notifyTrackableChanged) {
                    ChangeTracker?.SaveChanges(notifyTrackableChanged);
                }
            }
        }
        finally {
            Semaphore.Release();
        }
    }

    public T? Get<T>() {
        lock (Lock) {
            try {
                if (Settings.LifeTime!.Type == EntryLifetimeType.NoLifetime) {
                    // container.AddOrUpdate called and not saved yet
                    // Then Value is set but file is not written yet
                    if (Value is not null) {
                        return (T?)Value;
                    }

                    if (!FileSnapshot.TryCreate(Filename, out FileSnapshot? file)) {
                        return default;
                    }

                    return GetInternal<T>(file);
                }
                else {
                    if (Value is null) {
                        if (!FileSnapshot.TryCreate(Filename, out FileSnapshot? file)) {
                            return default;
                        }

                        Value = GetInternal<T>(file);
                    }

                    return (T?)Value;
                }
            }
            catch {
                return default;
            }
        }
    }

    private T? GetInternal<T>(FileSnapshot file) {
        T? value;

        if (Settings.EncryptionEnabled) {
            Result<IKey> keyResult = Settings.ContainerCryptography!.GetEntryKey.Invoke();
            IKey key = keyResult.GetValueOrThrow();

            value = xmlService.DecryptXml<T>(file, new Cryptographer(), new CryptographyInput {
                Key = key,
                Mode = Settings.ContainerCryptography.CryptographyMode
            });
        }
        else {
            value = xmlService.ReadXml<T>(file);
        }

        if (value is INotifyTrackableChanged notifyTrackableChanged) {
            ChangeTracker?.Track(notifyTrackableChanged);
            ChangeTracker?.HookStateChanged(notifyTrackableChanged);
        }

        return value;
    }

    public async Task<T?> GetAsync<T>() {
        await Semaphore.WaitAsync();

        try {
            if (Settings.LifeTime!.Type == EntryLifetimeType.NoLifetime) {
                // container.AddOrUpdate called and not saved yet
                // Then Value is set but file is not written yet
                if (Value is not null) {
                    return (T?)Value;
                }

                if (!FileSnapshot.TryCreate(Filename, out FileSnapshot? file)) {
                    return default;
                }

                return await GetAsyncInternal<T>(file);
            }
            else {
                if (Value is null) {
                    if (!FileSnapshot.TryCreate(Filename, out FileSnapshot? file)) {
                        return default;
                    }

                    Value = await GetAsyncInternal<T>(file);
                }
            }

            return (T?)Value;
        }
        catch {
            return default;
        }
        finally {
            Semaphore.Release();
        }
    }

    private async Task<T?> GetAsyncInternal<T>(FileSnapshot file) {
        T? value;

        if (Settings.EncryptionEnabled) {
            Result<IKey> keyResult = await Settings.ContainerCryptography!.GetEntryKeyAsync.Invoke();

            IKey key = keyResult.GetValueOrThrow();

            value = xmlService.DecryptXml<T>(file, new Cryptographer(), new CryptographyInput {
                Key = key,
                Mode = Settings.ContainerCryptography.CryptographyMode
            });
        }
        else {
            value = await xmlService.ReadXmlAsync<T>(file);
        }

        if (value is INotifyTrackableChanged notifyTrackableChanged) {
            ChangeTracker?.Track(notifyTrackableChanged);
            ChangeTracker?.HookStateChanged(notifyTrackableChanged);
        }

        return value;
    }

    public void Save<T>() {
        lock (Lock) {
            if (Value is T tValue) {
                if (Settings.EncryptionEnabled) {
                    Result<IKey> keyResult = Settings.ContainerCryptography!.GetEntryKey.Invoke();

                    IKey key = keyResult.GetValueOrThrow();

                    xmlService.EncryptXml(FileSnapshot.Create(Filename, true), tValue, new Cryptographer(), new CryptographyInput {
                        Key = key,
                        Mode = Settings.ContainerCryptography.CryptographyMode
                    });
                }
                else {
                    xmlService.WriteXml(FileSnapshot.Create(Filename, true), tValue);
                }

                if (Value is INotifyTrackableChanged notifyTrackableChanged) {
                    ChangeTracker?.Track(notifyTrackableChanged);
                    ChangeTracker?.HookStateChanged(notifyTrackableChanged);
                }
            }
            else {
                throw new InvalidOperationException("Cannot save, entry does not equal given type.");
            }
        }
    }

    public async Task SaveAsync<T>() {
        await Semaphore.WaitAsync();
        try {

            if (Value is T tValue) {
                if (Settings.EncryptionEnabled) {
                    Result<IKey> keyResult = await Settings.ContainerCryptography!.GetEntryKeyAsync.Invoke();

                    IKey key = keyResult.GetValueOrThrow();

                    xmlService.EncryptXml(FileSnapshot.Create(Filename, true), tValue, new Cryptographer(), new CryptographyInput {
                        Key = key,
                        Mode = Settings.ContainerCryptography.CryptographyMode
                    });
                }
                else {
                    await xmlService.WriteXmlAsync(FileSnapshot.Create(Filename, true), tValue);
                }

                if (Value is INotifyTrackableChanged notifyTrackableChanged) {
                    ChangeTracker?.Track(notifyTrackableChanged);
                    ChangeTracker?.HookStateChanged(notifyTrackableChanged);
                }
            }
            else {
                throw new InvalidOperationException("Cannot save, entry does not equal given type.");
            }
        }
        finally {
            Semaphore.Release();
        }
    }

    protected override void OnLifetimeOver(object sender, TimeSpan fullTime) {
        Save();

        if (Value is IDisposable disposable) {
            disposable.Dispose();
        }
        else {
            Value = null;
        }
    }
}
