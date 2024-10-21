using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HBLibrary.Interface.Plugins.Attributes;
[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
public class PluginAttribute : Attribute {
    public Type BaseType { get; set; }
    public PluginAttribute(Type baseType) {
        BaseType = baseType;
    }
}

[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
public class PluginAttribute<T> : Attribute where T : class {
    public Type BaseType { get; set; }
    public PluginAttribute() {
        BaseType = typeof(T);
    }
}

