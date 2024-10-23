using HBLibrary.Interface.Core.ChangeTracker;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
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

    public bool LastValueChanged(string key, object? value) {
        if (history.TryGetValue(key, out SortedList<DateTime, object?>? list)) {
            if (value is null && list.LastOrDefault().Value is null) {
                return false;
            }

            return value?.Equals(list.LastOrDefault().Value) ?? false;
        }

        return false;
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
            return list.LastOrDefault().Value;
        }

        return null;
    }

    public IReadOnlyDictionary<string, object?> GetNewestValues() {
        return history.ToDictionary(e => e.Key, e => e.Value.LastOrDefault().Value);
    }

    public IReadOnlyDictionary<DateTime, object?>? GetHistory(string key) {
        return history[key];
    }

    public bool TryAddOrUpdate(string key, object? value) {
        if (history.TryGetValue(key, out SortedList<DateTime, object?>? list)) {
            if (value is null && list.LastOrDefault().Value is null || (value?.Equals(list.LastOrDefault().Value) ?? true)) {
                return false;
            }

            list.Add(DateTime.UtcNow, value);
            return true;
        }
        else {
            history[key] = new SortedList<DateTime, object?> { [DateTime.UtcNow] = value };
            return true;
        }
    }
}
