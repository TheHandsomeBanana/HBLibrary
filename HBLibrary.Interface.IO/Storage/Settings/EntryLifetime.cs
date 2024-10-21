using HBLibrary.Core.Timer;
using System.Text.Json.Serialization;

namespace HBLibrary.Interface.IO.Storage.Settings;
public class EntryLifetime {
    public EntryLifetimeType Type { get; set; }
    public TimeSpan? LifeTime { get; set; }
    public bool? ResetOnAccess { get; set; }

    [JsonIgnore]
    public AsyncCountdown? Countdown { get; init; }

    public static EntryLifetime CreateSingletonLifetime() {
        return new EntryLifetime {
            Type = EntryLifetimeType.Singleton
        };
    }

    public static EntryLifetime CreateTimedLifetime(TimeSpan lifetime, bool resetOnAccess = true) {
        return new EntryLifetime {
            Type = EntryLifetimeType.Timed,
            LifeTime = lifetime,
            ResetOnAccess = resetOnAccess,
            Countdown = new AsyncCountdown(lifetime)
        };
    }

    public static EntryLifetime CreateWithoutLifetime() {
        return new EntryLifetime {
            Type = EntryLifetimeType.NoLifetime,
        };
    }

}

public enum EntryLifetimeType {
    Singleton,
    Timed,
    NoLifetime
}
