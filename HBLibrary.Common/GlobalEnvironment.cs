﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HBLibrary.Common;
public static class GlobalEnvironment {
    public static readonly string ApplicationDataBasePath
        = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\HB";

    public static Encoding Encoding { get; set; } = Encoding.UTF8;

    static GlobalEnvironment() {
        Directory.CreateDirectory(ApplicationDataBasePath);
    }
}
