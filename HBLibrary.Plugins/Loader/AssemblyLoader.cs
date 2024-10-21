using HBLibrary.Common.Plugins;
using HBLibrary.DataStructures;
using HBLibrary.Interface.Plugins;
using HBLibrary.Interface.Plugins.Loader;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;


namespace HBLibrary.Plugins.Loader;

public class AssemblyLoader : IAssemblyLoader {

    public Result<IAssemblyContext> LoadAssembly(string assemblyPath) {
        try {
            return AssemblyContext.CreateSingle(assemblyPath);
        }
        catch (Exception ex) {
            return ex;
        }
    }

    public Result<IAssemblyContext> LoadAssemblies(string[] assemblyPaths) {
        try {
            return AssemblyContext.Create(assemblyPaths);
        }
        catch (Exception ex) {
            return ex;
        }
    }

    public void Unload(IAssemblyContext context) {
        WeakReference weakReference = new WeakReference(context);
        context.Dispose();
    }
}
