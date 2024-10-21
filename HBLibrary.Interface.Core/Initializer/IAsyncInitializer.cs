using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HBLibrary.Interface.Core.Initializer;
public interface IAsyncInitializer {
    public Task InitializeAsync();
    public bool IsInitialized { get; }
}
