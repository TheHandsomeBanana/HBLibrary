using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using HBLibrary.Common.Results;


namespace HBLibrary.Common.Plugins.Loader;

public class AssemblyLoader : IAssemblyLoader {

    public Result<AssemblyContext> LoadAssembly(string assemblyPath) {
        try {
            return AssemblyContext.CreateSingle(assemblyPath);
        }
        catch (Exception ex) {
            return ex;
        }
    }

    public Result<AssemblyContext> LoadAssemblies(string[] assemblyPaths) {
        try {
            return AssemblyContext.Create(assemblyPaths);
        }
        catch (Exception ex) {
            return ex;
        }
    }

    public void Unload(AssemblyContext context) {
        context.Dispose();
    }
}
