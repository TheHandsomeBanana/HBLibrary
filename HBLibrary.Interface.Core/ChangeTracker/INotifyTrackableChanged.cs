using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HBLibrary.Interface.Core.ChangeTracker;
public delegate void TrackableChanged(object? sender, TrackedChanges trackedChanges);
public interface INotifyTrackableChanged {
    public event TrackableChanged? TrackableChanged;
}
