using HBLibrary.Common.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace HBLibrary.Common.Plugins.Loader;
public interface IAssemblyLoader {
    public Result<AssemblyContext> LoadAssembly(string assemblyPath);
    public Result<AssemblyContext> LoadAssemblies(string[] assemblyPaths);
    public void Unload(AssemblyContext assemblyContext);
}
