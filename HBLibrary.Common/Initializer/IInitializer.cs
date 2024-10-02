using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HBLibrary.Common.Initializer;
public interface IInitializer {
    public void Initialize();
    public bool IsInitialized { get; }
}
