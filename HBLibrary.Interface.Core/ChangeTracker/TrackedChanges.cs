using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HBLibrary.Interface.Core.ChangeTracker;
public class TrackedChanges {
    public required string Name { get; init; }
    public required object? Value { get; init; }
}
