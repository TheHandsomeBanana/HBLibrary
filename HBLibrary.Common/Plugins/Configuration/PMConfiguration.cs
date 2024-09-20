using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HBLibrary.Common.Plugins.Configuration;
internal class PMConfiguration : IPMConfiguration {
    public required bool CachePluginInstances { get; set; }
    public required string Location { get; set; }
    public bool OverrideAssemblies { get; set; } = true;
    public bool PreloadAssemblies { get; set; } = true;
}
