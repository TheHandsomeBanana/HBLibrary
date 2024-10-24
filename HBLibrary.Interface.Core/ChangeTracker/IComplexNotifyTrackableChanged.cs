using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HBLibrary.Interface.Core.ChangeTracker;

public interface IComplexNotifyTrackableChanged : ITrackable {
    public IChangeTracker? ChangeTracker { get; set; }
}
