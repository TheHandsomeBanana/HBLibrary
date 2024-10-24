using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HBLibrary.Interface.Core.ChangeTracker;
public interface ITrackedItem : IDisposable {
    public event Action<bool>? TrackedItemUpdated;
    public bool TrackedItemUpdatedIsNull { get; }
    public IChangeSetHistory? History { get; }
    public ITrackable Item { get; }
    public bool HasChanges { get; }

    public void SaveChanges();
}
