using HBLibrary.Interface.Core.ChangeTracker;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace HBLibrary.Wpf.Models;
public abstract class TrackableModel : INotifyTrackableChanged {
    public event TrackableChanged? TrackableChanged;

    protected void NotifyTrackableChanged([CallerMemberName] string propertyName = "") {
        if (TrackableChanged is not null) {
            object? propertyValue = this.GetType().GetProperty(propertyName)?.GetValue(this);
            TrackableChanged.Invoke(this, new TrackedChanges {
                Name = propertyName,
                Value = propertyValue
            });
        }
    }

    protected void NotifyTrackableChanged(object? propertyValue, [CallerMemberName] string propertyName = "") {
        TrackableChanged?.Invoke(this, new TrackedChanges {
            Name = propertyName,
            Value = propertyValue
        });
    }

    protected void TrackCollectionItem(INotifyTrackableChanged notifyTrackableChanged) {
        notifyTrackableChanged.TrackableChanged += Item_TrackableChanged;
    }

    protected void UntrackCollectionItem(INotifyTrackableChanged notifyTrackableChanged) {
        notifyTrackableChanged.TrackableChanged -= Item_TrackableChanged;
    }

    private void Item_TrackableChanged(object? sender, TrackedChanges trackedChanges) {
        TrackableChanged?.Invoke(sender, trackedChanges);
    }

    protected void CollectionChanged(object? sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e) {
        if (e.NewItems is not null) {
            foreach (INotifyTrackableChanged newItem in e.NewItems) {
                TrackCollectionItem(newItem);
            }
        }

        if (e.OldItems is not null) {
            foreach (INotifyTrackableChanged oldItem in e.OldItems) {
                UntrackCollectionItem(oldItem);
            }
        }
    }
}

