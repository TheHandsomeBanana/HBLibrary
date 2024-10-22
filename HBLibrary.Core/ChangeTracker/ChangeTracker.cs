using HBLibrary.Interface.Core.ChangeTracker;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HBLibrary.Core.ChangeTracker;
public sealed class ChangeTracker : IChangeTracker {
    private readonly List<ITrackedItem> changeSets = [];
    public IReadOnlyList<ITrackedItem> TrackedItems => changeSets;

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

        changeSets.Add(new TrackedItem(entity));
    }

    public void Untrack(INotifyTrackableChanged entity) {
        if (!IsTracked(entity)) {
            return;
        }

        ITrackedItem foundChangeSet = changeSets.First(e => ReferenceEquals(entity, e.Item));
        foundChangeSet.Dispose();
        changeSets.Remove(foundChangeSet);
    }

    public void SaveChanges(INotifyTrackableChanged entity) {
        ITrackedItem? changeSet = Get(entity);
        changeSet?.SaveChanges();
    }

    public void SaveAllChanges() {
        foreach (ITrackedItem changeSet in changeSets) {
            changeSet.SaveChanges();
        }
    }

    public void UntrackAll() {
        foreach (ITrackedItem changeSet in changeSets) {
            changeSet.Dispose();
        }

        changeSets.Clear();
    }

    public void Dispose() {
        UntrackAll();
    }

  
}
