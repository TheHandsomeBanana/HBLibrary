using HBLibrary.Interface.Core.ChangeTracker;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HBLibrary.Core.ChangeTracker;
public sealed class TrackedItem : ITrackedItem {
    private readonly LatestTracks capturedTracks = new LatestTracks();
    public IChangeSetHistory? History { get; } 
    public ITrackable Item { get; }
    public DateTime LastChangedAt { get; private set; }
    public bool HasChanges { get; private set; }

    public bool TrackedItemUpdatedIsNull => TrackedItemUpdated is null;

    public event Action<bool>? TrackedItemUpdated;

    public TrackedItem(ITrackable trackedItem) {
        if(trackedItem.UseTrackingHistory) {
            History = new ChangeSetHistory();
        }

        Item = trackedItem;
        Item.TrackableChanged += TrackedItem_TrackableChanged;
    }


    private void TrackedItem_TrackableChanged(object? sender, TrackedChanges trackedChanges) {
        DateTime changedAt = DateTime.UtcNow;



        bool updated = capturedTracks.AddOrUpdateIfNotEquals(trackedChanges.Name, trackedChanges.Value);

        if (updated) {
            History?.AddOrUpdate(trackedChanges.Name, trackedChanges.Value);

            LastChangedAt = changedAt;
            HasChanges = true;
            TrackedItemUpdated?.Invoke(true);
        }

    }

    public void Dispose() {
        Item.TrackableChanged -= TrackedItem_TrackableChanged;
    }

    public void SaveChanges() {
        HasChanges = false;
        TrackedItemUpdated?.Invoke(false);
    }
}
