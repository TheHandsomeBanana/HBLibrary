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

    public ITrackedItem? Get(ITrackable entity);
    public bool IsTracked(ITrackable entity);
    public void Track(ITrackable entity);
    public void Untrack(ITrackable entity);
    public void UntrackAll();
    public void SaveChanges(ITrackable entity);
    public void SaveAllChanges();
    public void HookStateChanged();
    public void HookStateChanged(ITrackable entity);
}
