using HBLibrary.Interface.Core.ChangeTracker;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HBLibrary.Core.ChangeTracker;
public sealed class TrackedItem : ITrackedItem {
    public IChangeSetHistory History { get; } = new ChangeSetHistory();
    public INotifyTrackableChanged Item { get; }
    public DateTime LastChangedAt { get; private set; }
    public bool HasChanges { get; private set; }


    public TrackedItem(INotifyTrackableChanged trackedItem) {
        Item = trackedItem;
        Item.TrackableChanged += TrackedItem_TrackableChanged;
    }

    private void TrackedItem_TrackableChanged(object? sender, TrackedChanges trackedChanges) {
        DateTime changedAt = DateTime.UtcNow;
        HasChanges = true;
        LastChangedAt = changedAt;
        History.AddOrUpdate(trackedChanges.Name, trackedChanges.Value);
    }

    public void Dispose() {
        Item.TrackableChanged -= TrackedItem_TrackableChanged;
    }

    public void SaveChanges() {
        HasChanges = false;
    }
}
