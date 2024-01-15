using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HBLibrary.NetFramework.Code.Analysis {
    public interface IAnalyserRegistry {
        IAnalyserFactory Factory { get; }
    }
}
