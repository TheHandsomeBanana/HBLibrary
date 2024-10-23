using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HBLibrary.Interface.Core.ChangeTracker;
public delegate void TrackableChanged(object? sender, TrackedChanges trackedChanges);
/// <summary>
/// Nested Properties must be named different
/// </summary>
public interface INotifyTrackableChanged {
    public event TrackableChanged? TrackableChanged;
}
