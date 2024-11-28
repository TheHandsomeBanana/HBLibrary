using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HBLibrary.Workflows;
[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
public class WFPropertyAttribute : Attribute {
    public string QualifiedName { get; set; }

    public WFPropertyAttribute(string qualifiedName) {
        QualifiedName = qualifiedName;
    }
}
