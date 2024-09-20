using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HBLibrary.Common.Plugins;
public class PluginType : IEquatable<PluginType> {
    public required Type BaseType { get; set; }
    public required Type ConcreteType { get; set; }
    public required string TypeName { get; set; }
    public string? Description { get; set; }

    public bool Equals(PluginType? other) {
        return ConcreteType == other?.ConcreteType;
    }

    public override bool Equals(object? obj) {
        return obj is PluginType pt && Equals(pt);
    }

    public override int GetHashCode() {
        return ConcreteType.GetHashCode();
    }
}
