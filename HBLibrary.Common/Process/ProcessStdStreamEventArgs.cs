using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HBLibrary.Common.Process;
public class ProcessStdStreamEventArgs : EventArgs {
    public string Data { get; set; }

    public ProcessStdStreamEventArgs(string data) {
        Data = data;
    }
}
