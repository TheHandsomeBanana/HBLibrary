using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Immutable;

#if NET5_0_OR_GREATER
using System.Runtime.Loader;
#endif

namespace HBLibrary.Common.Plugins.Loader;

#if NET5_0_OR_GREATER
public sealed class AssemblyContext : IDisposable {
    private readonly AssemblyLoadContext context;
    private readonly AssemblyName initialAssemblyName;

    private AssemblyContext(string[] assemblyFullPaths) {
        this.initialAssemblyName = AssemblyName.GetAssemblyName(assemblyFullPaths[0]);

        context = new AssemblyLoadContext("LoadContext", true);
        foreach (string assemblyFullPath in assemblyFullPaths) {
            context.LoadFromAssemblyPath(assemblyFullPath);
        }
    }

    public static AssemblyContext CreateSingle(string assemblyFullPath) {
        return new AssemblyContext([assemblyFullPath]);
    }

    public static AssemblyContext Create(string[] assemblyFullPath) {
        return new AssemblyContext(assemblyFullPath);
    }

    public Assembly GetFirst() {
        return context.LoadFromAssemblyName(initialAssemblyName);
    }

    public Assembly? Get(string assemblyFullPath) {
        return Get(AssemblyName.GetAssemblyName(assemblyFullPath));
    }

    public IEnumerable<Assembly> QueryAll() {
        return context.Assemblies;
    }

    public Assembly[] GetAll() {
        return context.Assemblies.ToArray();
    }

    public Assembly? Get(AssemblyName assemblyName) {
        return context.Assemblies.FirstOrDefault(e => {
            AssemblyName? name = e.GetName();

            return name.Name == assemblyName.Name && name.Version == assemblyName.Version &&
                Enumerable.SequenceEqual(name.GetPublicKeyToken() ?? [], assemblyName.GetPublicKeyToken() ?? []);
        });
    }

    public void Dispose() {
        context.Unload();
        GC.Collect();
        GC.WaitForPendingFinalizers();
    }
}
#endif

#if NET472_OR_GREATER
public class AssemblyContext {
    private readonly AppDomain domain;
    private readonly AssemblyName initialAssemblyName;

    private AssemblyContext(string[] assemblyFullPaths) {
        this.initialAssemblyName = AssemblyName.GetAssemblyName(assemblyFullPaths[0]);

        AppDomainSetup setup = new AppDomainSetup {
            ApplicationBase = AppDomain.CurrentDomain.BaseDirectory
        };

        domain = AppDomain.CreateDomain("AssemblyContextDomain", null, setup);

        foreach(string assemblyFullPath in assemblyFullPaths) {
            domain.Load(AssemblyName.GetAssemblyName(assemblyFullPath));
        }
    }

    public static AssemblyContext CreateSingle(string assemblyFullPath) {
        return new AssemblyContext([assemblyFullPath]);
    }
    
    public static AssemblyContext Create(string[] assemblyFullPath) {
        return new AssemblyContext(assemblyFullPath);
    }


    public Assembly GetFirst() {
        return domain.Load(initialAssemblyName);
    }

    public Assembly? Get(string assemblyFullPath) {
        return Get(AssemblyName.GetAssemblyName(assemblyFullPath));
    }

    public Assembly? Get(AssemblyName assemblyName) {
        return domain.GetAssemblies().FirstOrDefault(e => {
            AssemblyName? name = e.GetName();

            return name.Name == assemblyName.Name && name.Version == assemblyName.Version &&
                Enumerable.SequenceEqual(name.GetPublicKeyToken() ?? [], assemblyName.GetPublicKeyToken() ?? []);
        });
    }

    public IEnumerable<Assembly> QueryAll() {
        return domain.GetAssemblies();
    }

    public Assembly[] GetAll() {
        return domain.GetAssemblies();
    }

    public void Dispose() {
        AppDomain.Unload(domain);
        GC.Collect();
        GC.WaitForPendingFinalizers();
    }
}
#endif
