using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HBLibrary.NetFramework.Code.Analysis.Exceptions {
    public class AnalyserException : Exception {
        public AnalyserException(string message) : base(message) {
        }

        public static AnalyserException AnalyserRegistered(string name)
            => new AnalyserException($"{name} already registered.");
    }
}
