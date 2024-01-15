using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HBLibrary.NetFramework.Common {
    public static class GlobalEnvironment {
        public static readonly string ApplicationDataBasePath
            = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\HB_Application_Data";

        public static Encoding Encoding { get; set; } = Encoding.UTF8;

        static GlobalEnvironment() {
            Directory.CreateDirectory(ApplicationDataBasePath);
        }
    }
}
