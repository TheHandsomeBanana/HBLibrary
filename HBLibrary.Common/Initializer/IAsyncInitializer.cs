﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HBLibrary.Common.Initializer;
public interface IAsyncInitializer {
    public Task InitializeAsync();
    public bool IsInitialized { get; }
}
