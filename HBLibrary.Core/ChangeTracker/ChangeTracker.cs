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

    public bool HasActiveChanges { get; private set; } = false;

    public ITrackedItem? Get(ITrackable entity) {
        return TrackedItems.FirstOrDefault(e => ReferenceEquals(e.Item, entity));
    }

    public bool IsTracked(ITrackable entity) {
        return TrackedItems.Any(e => ReferenceEquals(e.Item, entity));
    }

    public void Track(ITrackable entity) {
        if (IsTracked(entity)) {
            return;
        }

        TrackedItem trackedItem = new TrackedItem(entity);
        trackedItem.TrackedItemUpdated += TrackedItemStateChanged;

        trackedItems.Add(trackedItem);
    }

    public void Untrack(ITrackable entity) {
        if (!IsTracked(entity)) {
            return;
        }

        ITrackedItem trackedItem = trackedItems.First(e => ReferenceEquals(entity, e.Item));
        trackedItem.TrackedItemUpdated -= TrackedItemStateChanged;
        trackedItem.Dispose();
        trackedItems.Remove(trackedItem);
    }

    public void SaveChanges(ITrackable entity) {
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
            trackedItem.TrackedItemUpdated -= TrackedItemStateChanged;
            trackedItem.Dispose();
        }

        trackedItems.Clear();
    }

    public void Dispose() {
        UntrackAll();
    }

    public void HookStateChanged() {
        foreach (ITrackedItem trackedItem in trackedItems) {
            if (trackedItem.TrackedItemUpdatedIsNull) {
                trackedItem.TrackedItemUpdated += TrackedItemStateChanged;
            }
        }
    }

    public void HookStateChanged(ITrackable notifyTrackableChanged) {
        ITrackedItem? trackedItem = Get(notifyTrackableChanged);

        if (trackedItem?.TrackedItemUpdatedIsNull ?? false) {
            trackedItem.TrackedItemUpdated += TrackedItemStateChanged;
        }
    }

    private void TrackedItemStateChanged(bool state) {
        if (HasActiveChanges == state) {
            return;
        }

        HasActiveChanges = trackedItems.Any(e => e.HasChanges);

        ChangeTrackerStateChanged?.Invoke(HasActiveChanges);
    }
}
