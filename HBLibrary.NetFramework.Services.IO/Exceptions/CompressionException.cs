using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HBLibrary.NetFramework.Services.IO.Exceptions {
    public class CompressionException : Exception {
        public CompressionException(string message) : base(message) {
        }
    }
}
