using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HBLibrary.Common.Plugins.Configuration;
public interface IPMConfiguration {
    public string Location { get; }
    public bool OverrideAssemblies { get; }
    public bool PreloadAssemblies { get; }
    public bool CachePluginInstances { get; }
}
