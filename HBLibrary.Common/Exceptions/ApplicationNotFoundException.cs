﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HBLibrary.Common.Exceptions;
public class ApplicationNotFoundException : Exception {
    public ApplicationNotFoundException(string application) : base($"{application} not found on the system.") { }

    public ApplicationNotFoundException(string application, string message)
        : base($"{application} not found on the system. {message}") { }
}
