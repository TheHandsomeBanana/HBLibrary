using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HBLibrary.Interface.Core.ChangeTracker;
public interface IChangeTracker : IDisposable {
    public IReadOnlyList<ITrackedItem> TrackedItems { get; }
    public bool HasActiveChanges { get; }
    public event Action<bool>? ChangeTrackerStateChanged;

    public ITrackedItem? Get(INotifyTrackableChanged entity);
    public bool IsTracked(INotifyTrackableChanged entity);
    public void Track(INotifyTrackableChanged entity);
    public void Untrack(INotifyTrackableChanged entity);
    public void UntrackAll();
    public void SaveChanges(INotifyTrackableChanged entity);
    public void SaveAllChanges();
    public void HookStateChanged();
    public void HookStateChanged(INotifyTrackableChanged entity);
}
