using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HBLibrary.Common.Extensions;
public static class TypeExtensions {
    public static string GuidString(this Type type) => type.GUID.ToString();
}
