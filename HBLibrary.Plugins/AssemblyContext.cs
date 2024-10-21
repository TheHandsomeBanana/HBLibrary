using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Immutable;
using System.IO;

/* Unmerged change from project 'HBLibrary.Plugins (net8.0)'
Added:
using HBLibrary.Interface.Plugins;
*/
using HBLibrary.Interface.Plugins;


#if NET5_0_OR_GREATER
using System.Runtime.Loader;
#endif

namespace HBLibrary.Common.Plugins;

#if NET5_0_OR_GREATER
public sealed class AssemblyContext : IAssemblyContext {
    private readonly AssemblyLoadContext context;
    private readonly AssemblyName initialAssemblyName;

    private AssemblyContext(string initialAssemblyFullPath) {
        this.initialAssemblyName = AssemblyName.GetAssemblyName(initialAssemblyFullPath);
        context = new AssemblyLoadContext("LoadContext", true);

    }


    public static AssemblyContext CreateSingle(string assemblyFullPath) {
        AssemblyContext context = new AssemblyContext(assemblyFullPath);

        byte[] assemblyBuffer = File.ReadAllBytes(assemblyFullPath);
        using MemoryStream memoryStream = new MemoryStream(assemblyBuffer);
        context.context.LoadFromStream(memoryStream);
        return context;
    }

    public static AssemblyContext Create(string[] assemblyFullPaths) {
        AssemblyContext context = new AssemblyContext(assemblyFullPaths[0]);

        foreach (string path in assemblyFullPaths) {
            byte[] assemblyBuffer = File.ReadAllBytes(path);
            using MemoryStream memoryStream = new MemoryStream(assemblyBuffer);
            context.context.LoadFromStream(memoryStream);
        }

        return context;
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
        context.Unload(); // Managed by GC
        GC.Collect();
        GC.WaitForPendingFinalizers();
        GC.Collect(); // Try to force unload
    }
}
#endif

#if NET472_OR_GREATER
public class AssemblyContext : IAssemblyContext {
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
