using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HBLibrary.Services.IO.Exceptions;
public class WinRARException : Exception {
    public WinRARException(string? message) : base(message) {
    }
}
