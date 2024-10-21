using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Immutable;
using System.IO;
namespace HBLibrary.Common.Plugins;

public interface IAssemblyContext : IDisposable {
    public Assembly GetFirst();
    public Assembly? Get(string assemblyFullPath);
    public IEnumerable<Assembly> QueryAll();
    public Assembly[] GetAll();
    public Assembly? Get(AssemblyName assemblyName);
}
