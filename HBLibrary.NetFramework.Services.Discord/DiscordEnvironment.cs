using HBLibrary.NetFramework.Common;
using System.IO;

namespace HBLibrary.NetFramework.Services.Discord {
    public static class DiscordEnvironment {
        public static readonly string BasePath = GlobalEnvironment.ApplicationDataBasePath + "\\Discord.Net";
        public static readonly string LogPath = BasePath + "\\Logs";
        public static readonly string CachePath = BasePath + "\\Cache";

        public static readonly string CacheExtension = ".hbdc";
        static DiscordEnvironment() {
            Directory.CreateDirectory(BasePath);
            Directory.CreateDirectory(LogPath);
            Directory.CreateDirectory(CachePath);
        }
    }
}
