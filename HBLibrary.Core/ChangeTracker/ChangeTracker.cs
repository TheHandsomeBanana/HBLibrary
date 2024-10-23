using HBLibrary.Interface.Core.ChangeTracker;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HBLibrary.Core.ChangeTracker;
public sealed class ChangeTracker : IChangeTracker {
    private readonly List<ITrackedItem> trackedItems = [];

    public event Action<bool>? ChangeTrackerStateChanged;

    public IReadOnlyList<ITrackedItem> TrackedItems => trackedItems;

    public bool HasActiveChanges => TrackedItems.Any(e => e.HasChanges);

    public ITrackedItem? Get(INotifyTrackableChanged entity) {
        return TrackedItems.FirstOrDefault(e => ReferenceEquals(e.Item, entity));
    }

    public bool IsTracked(INotifyTrackableChanged entity) {
        return TrackedItems.Any(e => ReferenceEquals(e.Item, entity));
    }

    public void Track(INotifyTrackableChanged entity) {
        if (IsTracked(entity)) {
            return;
        }

        TrackedItem trackedItem = new TrackedItem(entity);
        trackedItems.Add(trackedItem);
    }

    public void Untrack(INotifyTrackableChanged entity) {
        if (!IsTracked(entity)) {
            return;
        }

        ITrackedItem trackedItem = trackedItems.First(e => ReferenceEquals(entity, e.Item));
        trackedItem.TrackedItemStateChanged -= ChangeTrackerStateChanged;
        trackedItem.Dispose();
        trackedItems.Remove(trackedItem);
    }

    public void SaveChanges(INotifyTrackableChanged entity) {
        ITrackedItem? trackedItem = Get(entity);
        trackedItem?.SaveChanges();
    }

    public void SaveAllChanges() {
        foreach (ITrackedItem trackedItem in trackedItems) {
            trackedItem.SaveChanges();
        }
    }

    public void UntrackAll() {
        foreach (ITrackedItem trackedItem in trackedItems) {
            trackedItem.TrackedItemStateChanged -= ChangeTrackerStateChanged;
            trackedItem.Dispose();
        }

        trackedItems.Clear();
    }

    public void Dispose() {
        UntrackAll();
    }

    public void HookStateChanged() {
        foreach (ITrackedItem trackedItem in trackedItems) {
            if (trackedItem.TrackedItemStateChangedIsNull) {
                trackedItem.TrackedItemStateChanged += ChangeTrackerStateChanged;
            }
        }
    }

    public void HookStateChanged(INotifyTrackableChanged notifyTrackableChanged) {
        ITrackedItem? trackedItem = Get(notifyTrackableChanged);

        if (trackedItem?.TrackedItemStateChangedIsNull ?? false) {
            trackedItem.TrackedItemStateChanged += ChangeTrackerStateChanged;
        }
    }
}
