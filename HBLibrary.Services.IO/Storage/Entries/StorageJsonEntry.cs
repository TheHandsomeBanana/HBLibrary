using HBLibrary.Common;
using HBLibrary.Common.Security;
using HBLibrary.Common.Security.Keys;
using HBLibrary.Services.IO.Exceptions;
using HBLibrary.Services.IO.Json;
using HBLibrary.Services.IO.Storage.Settings;

namespace HBLibrary.Services.IO.Storage.Entries;
internal class StorageJsonEntry : StorageEntry, IStorageEntry {
    private readonly IJsonFileService jsonService;

    internal StorageJsonEntry(IJsonFileService jsonService, string filename, StorageEntrySettings settings)
        : base(filename, StorageEntryContentType.Json, settings) {
        this.jsonService = jsonService;
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
        if (Settings.EncryptionEnabled) {
            Result<IKey> keyResult = Settings.ContainerCryptography!.GetEntryKey.Invoke();

            IKey key = keyResult.GetValueOrThrow();

            return jsonService.DecryptJson(type, file, new Cryptographer(), new CryptographyInput {
                Key = key,
                Mode = Settings.ContainerCryptography.CryptographyMode
            });
        }
        else {
            return jsonService.ReadJson(type, file);
        }
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
        if (Settings.EncryptionEnabled) {
            Result<IKey> keyResult = await Settings.ContainerCryptography!.GetEntryKeyAsync.Invoke();

            IKey key = keyResult.GetValueOrThrow();

            return jsonService.DecryptJson(type, file, new Cryptographer(), new CryptographyInput {
                Key = key,
                Mode = Settings.ContainerCryptography.CryptographyMode
            });
        }
        else {
            return await jsonService.ReadJsonAsync(type, file);
        }
    }

    public void Save() {
        lock (Lock) {
            if (Value is not null) {
                if (Settings.EncryptionEnabled) {
                    Result<IKey> keyResult = Settings.ContainerCryptography!.GetEntryKey.Invoke();

                    IKey key = keyResult.GetValueOrThrow();

                    jsonService.EncryptJson(Value.GetType(), FileSnapshot.Create(Filename, true), Value, new Cryptographer(), new CryptographyInput {
                        Key = key,
                        Mode = Settings.ContainerCryptography.CryptographyMode
                    });
                }
                else {
                    jsonService.WriteJson(Value.GetType(), FileSnapshot.Create(Filename, true), Value);
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

                    jsonService.EncryptJson(Value.GetType(), FileSnapshot.Create(Filename, true), Value, new Cryptographer(), new CryptographyInput {
                        Key = key,
                        Mode = Settings.ContainerCryptography.CryptographyMode
                    });
                }
                else {
                    await jsonService.WriteJsonAsync(Value.GetType(), FileSnapshot.Create(Filename, true), Value);
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
        if (Settings.EncryptionEnabled) {
            Result<IKey> keyResult = Settings.ContainerCryptography!.GetEntryKey.Invoke();

            IKey key = keyResult.GetValueOrThrow();

            return jsonService.DecryptJson<T>(file, new Cryptographer(), new CryptographyInput {
                Key = key,
                Mode = Settings.ContainerCryptography.CryptographyMode
            });
        }
        else {
            return jsonService.ReadJson<T>(file);
        }
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
        if (Settings.EncryptionEnabled) {
            Result<IKey> keyResult = await Settings.ContainerCryptography!.GetEntryKeyAsync.Invoke();

            IKey key = keyResult.GetValueOrThrow();
            return jsonService.DecryptJson<T>(file, new Cryptographer(), new CryptographyInput {
                Key = key,
                Mode = Settings.ContainerCryptography.CryptographyMode
            });
        }
        else {
            return await jsonService.ReadJsonAsync<T>(file);
        }
    }

    public void Save<T>() {
        lock (Lock) {
            if (Value is not null) {
                if (Value is T tValue) {
                    if (Settings.EncryptionEnabled) {
                        Result<IKey> keyResult = Settings.ContainerCryptography!.GetEntryKey.Invoke();

                        IKey key = keyResult.GetValueOrThrow();

                        jsonService.EncryptJson<T>(FileSnapshot.Create(Filename, true), tValue, new Cryptographer(), new CryptographyInput {
                            Key = key,
                            Mode = Settings.ContainerCryptography.CryptographyMode
                        });
                    }
                    else {
                        jsonService.WriteJson<T>(FileSnapshot.Create(Filename, true), tValue);
                    }
                }
                else {
                    throw new InvalidOperationException("Cannot save, entry does not equal given type.");
                }
            }
        }
    }

    public async Task SaveAsync<T>() {
        await Semaphore.WaitAsync();
        try {

            if (Value is not null) {
                if (Value is T tValue) {
                    if (Settings.EncryptionEnabled) {
                        Result<IKey> keyResult = await Settings.ContainerCryptography!.GetEntryKeyAsync.Invoke();

                        IKey key = keyResult.GetValueOrThrow();

                        jsonService.EncryptJson<T>(FileSnapshot.Create(Filename, true), tValue, new Cryptographer(), new CryptographyInput {
                            Key = key,
                            Mode = Settings.ContainerCryptography.CryptographyMode
                        });
                    }
                    else {
                        await jsonService.WriteJsonAsync<T>(FileSnapshot.Create(Filename, true), tValue);
                    }
                }
                else {
                    throw new InvalidOperationException("Cannot save, entry does not equal given type.");
                }
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

        Value = null;
    }
}
