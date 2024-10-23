using HBLibrary.Interface.Core.ChangeTracker;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HBLibrary.Wpf.Models;
public abstract class ComplexTrackableModel : TrackableModel, IComplexNotifyTrackableChanged {
    public IChangeTracker? ChangeTracker { get; set; }

}
