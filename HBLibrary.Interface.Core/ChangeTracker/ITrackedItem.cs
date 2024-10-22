using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HBLibrary.Interface.Core.ChangeTracker;
public interface ITrackedItem : IDisposable {
    public IChangeSetHistory History { get; }
    public INotifyTrackableChanged Item { get; }
    public bool HasChanges { get; }

    public void SaveChanges();
}
