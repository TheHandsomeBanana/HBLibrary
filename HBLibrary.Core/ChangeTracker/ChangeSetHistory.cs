using HBLibrary.Interface.Core.ChangeTracker;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HBLibrary.Core.ChangeTracker;
public class ChangeSetHistory : IChangeSetHistory {
    private readonly Dictionary<string, SortedList<DateTime, object?>> history = [];

    public object? this[string key] {
        get {
            return GetNewestValue(key);
        }
        set {
            AddOrUpdate(key, value);
        }
    }

    public void AddOrUpdate(string key, object? value) {
        if (history.TryGetValue(key, out SortedList<DateTime, object?>? list)) {
            list.Add(DateTime.UtcNow, value);
        }
        else {
            history[key] = new SortedList<DateTime, object?> { [DateTime.UtcNow] = value };
        }
    }

    public object? GetNewestValue(string key) {
        if (history.TryGetValue(key, out SortedList<DateTime, object?>? list)) {
            return list.LastOrDefault();
        }

        return null;
    }

    public IReadOnlyDictionary<string, object?> GetNewestValues() {
        return history.ToDictionary(e => e.Key, e => e.Value.LastOrDefault().Value);
    }

    public IReadOnlyDictionary<DateTime, object?>? GetHistory(string key) {
        return history[key];
    }
}
