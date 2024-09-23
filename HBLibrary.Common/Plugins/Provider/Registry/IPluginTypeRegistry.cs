using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HBLibrary.Common.Plugins.Provider.Registry;
public interface IPluginTypeRegistry {
    public void RegisterType(Type type);
    public Type? GetType(string typeName);
    public bool UnregisterType(string typeName);
    public void UnregisterTypes(AssemblyContext assemblyContext);
}
