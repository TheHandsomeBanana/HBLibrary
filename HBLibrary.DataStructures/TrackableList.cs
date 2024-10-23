using HBLibrary.Interface.Core.ChangeTracker;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HBLibrary.DataStructures;

public class TrackableList<T> : IList<T> where T : INotifyTrackableChanged {
    private readonly List<T> items = [];

    public event EventHandler<TrackedChanges>? TrackableChanged;

    public void Add(T item) {
        items.Add(item);
        SubscribeToTrackableChanged(item);
    }

    public void AddRange(IEnumerable<T> items) {
        this.items.AddRange(items);

        foreach (T item in items) {
            SubscribeToTrackableChanged(item);
        }
    }

    public bool Remove(T item) {
        bool removed = items.Remove(item);
        if (removed) {
            UnsubscribeFromTrackableChanged(item);
        }
        return removed;
    }

    public T this[int index] {
        get => items[index];
        set {
            var oldItem = items[index];
            UnsubscribeFromTrackableChanged(oldItem);
            items[index] = value;
            SubscribeToTrackableChanged(value);
        }
    }

    public int Count => items.Count;
    public bool IsReadOnly => false;

    public IEnumerator<T> GetEnumerator() => items.GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => items.GetEnumerator();

    public int IndexOf(T item) => items.IndexOf(item);
    public void Insert(int index, T item) {
        items.Insert(index, item);
        SubscribeToTrackableChanged(item);
    }

    public void RemoveAt(int index) {
        var item = items[index];
        items.RemoveAt(index);
        UnsubscribeFromTrackableChanged(item);
    }

    public void Clear() {
        foreach (var item in items) {
            UnsubscribeFromTrackableChanged(item);
        }
        items.Clear();
    }

    public bool Contains(T item) => items.Contains(item);
    public void CopyTo(T[] array, int arrayIndex) => items.CopyTo(array, arrayIndex);

    // Subscribe to TrackableChanged event for an item
    private void SubscribeToTrackableChanged(T item) {
        item.TrackableChanged += Item_TrackableChanged;
    }

    // Unsubscribe to prevent memory leaks
    private void UnsubscribeFromTrackableChanged(T item) {
        item.TrackableChanged -= Item_TrackableChanged;
    }

    // Propagate the trackable change event to the TrackableList level
    private void Item_TrackableChanged(object? sender, TrackedChanges e) {
        TrackableChanged?.Invoke(sender, e);
    }
}
