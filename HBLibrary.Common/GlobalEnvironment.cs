using System.Text;

namespace HBLibrary.Common;
public static class GlobalEnvironment {
    public static readonly string ApplicationDataBasePath
        = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\HB";

    public static readonly string IdentityPath = Path.Combine(ApplicationDataBasePath, "Identity");
    public static readonly string LogPath = Path.Combine(ApplicationDataBasePath, "Logs");

    public static Encoding Encoding { get; set; } = Encoding.UTF8;

    static GlobalEnvironment() {
        Directory.CreateDirectory(ApplicationDataBasePath);
        Directory.CreateDirectory(IdentityPath);
        Directory.CreateDirectory(LogPath);
    }
}
