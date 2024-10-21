using HBLibrary.DataStructures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace HBLibrary.Interface.Plugins.Loader;
public interface IAssemblyLoader {
    public Result<IAssemblyContext> LoadAssembly(string assemblyPath);
    public Result<IAssemblyContext> LoadAssemblies(string[] assemblyPaths);
    public void Unload(IAssemblyContext assemblyContext);
}
