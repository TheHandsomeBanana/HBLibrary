using HBLibrary.Interface.IO.Storage.Entries;
using HBLibrary.Interface.IO.Storage.Settings;

namespace HBLibrary.IO.Storage.Entries;
public abstract class StorageEntry {
    protected object Lock = new object();
    protected SemaphoreSlim Semaphore = new SemaphoreSlim(1);

    protected object? Value { get; set; }
    public StorageEntrySettings Settings { get; set; }
    public string Filename { get; }

    public Type? CurrentEntryType => Value?.GetType();
    public StorageEntryContentType ContentType { get; }


    public StorageEntry(string filename, StorageEntryContentType contentType, StorageEntrySettings settings) {
        Filename = filename;
        ContentType = contentType;
        Settings = settings;

        if (settings.LifeTime!.Countdown is not null) {
            settings.LifeTime.Countdown.CountdownCompleted += OnLifetimeOver;
        }
    }

    protected abstract void OnLifetimeOver(object sender, TimeSpan fullTime);

    public virtual void Set(object value) {
        if (Settings.LifeTime!.Type == EntryLifetimeType.NoLifetime) {
            return;
        }

        Value = value;
    }

    public virtual void Set<T>(T value) {
        if (Settings.LifeTime!.Type == EntryLifetimeType.NoLifetime) {
            return;
        }

        if (Value is not null) {
            if (Value is not T) {
                throw new InvalidOperationException("Cannot save, entry does not equal given type.");
            }
        }

        Value = value;
    }
}
