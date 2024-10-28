using HBLibrary.Interface.Core.ChangeTracker;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace HBLibrary.Wpf.Models;
public abstract class TrackableModel : ITrackable {
    public virtual bool UseTrackingHistory { get; } = false;

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
    
    protected void NotifyTrackableChanged(object? sender, object? propertyValue, [CallerMemberName] string propertyName = "") {
        TrackableChanged?.Invoke(sender, new TrackedChanges {
            Name = propertyName,
            Value = propertyValue
        });
    }
    
    protected void NotifyTrackableChanged(TrackedChanges trackedChanges, [CallerMemberName] string propertyName = "") {
        TrackableChanged?.Invoke(this, trackedChanges);
    }

    protected void NotifyTrackableChanged(object? sender, TrackedChanges trackedChanges, [CallerMemberName] string propertyName = "") {
        TrackableChanged?.Invoke(sender, trackedChanges);
    }
    
    protected void NotifyNestedTrackableChanged(object? sender, object? propertyValue, string containingClass, string propertyName) {
        TrackableChanged?.Invoke(sender, new TrackedChanges {
            Name = $"{containingClass}.{propertyName}",
            Value = propertyValue
        });
    }

    #region Protected methods for custom collections
    protected void TrackCollectionItem(string typeName, ITrackable notifyTrackableChanged) {
        notifyTrackableChanged.TrackableChanged += (sender, e) => Item_TrackableChanged(sender, typeName, e);
    }

    protected void UntrackCollectionItem(string typeName, ITrackable notifyTrackableChanged) {
        notifyTrackableChanged.TrackableChanged -= (sender, e) => Item_TrackableChanged(sender, typeName, e);
    }

    private void Item_TrackableChanged(object? sender, string typeName, TrackedChanges trackedChanges) {
        NotifyNestedTrackableChanged(sender, trackedChanges.Value, typeName, trackedChanges.Name);
    }

    protected void CollectionChanged(object? sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e) {
        if (e.NewItems is not null) {
            foreach (ITrackable newItem in e.NewItems) {
                TrackCollectionItem(e.NewItems[0]!.GetType().Name, newItem);
            }
        }

        if (e.OldItems is not null) {
            foreach (ITrackable oldItem in e.OldItems) {
                UntrackCollectionItem(e.OldItems[0]!.GetType().Name, oldItem);
            }
        }
    }
    #endregion
}

