﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HBLibrary.Interface.Core.Initializer;
public interface IInitializer {
    public void Initialize();
    public bool IsInitialized { get; }
}
