﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HBLibrary.Interface.Plugins.Attributes;
[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
public class PluginTypeNameAttribute : Attribute {
    public string TypeName { get; set; }

    public PluginTypeNameAttribute(string typeName) {
        TypeName = typeName;
    }
}
