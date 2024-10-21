using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HBLibrary.Interface.Plugins.Attributes;
[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
public class PluginDescriptionAttribute : Attribute {
    public string Description { get; set; }

    public PluginDescriptionAttribute(string description) {
        Description = description;
    }
}
