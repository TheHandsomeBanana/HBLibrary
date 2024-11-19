using HBLibrary.Wpf.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Windows.Media;

namespace HBLibrary.Wpf.Logging;
public class ListBoxLog {
    public int LineNumber { get; set; }
    public required string Message { get; set; }
    public required DateTime Timestamp { get; set; }
    public string? LogLevel { get; set; }
    public string? OwnerCategory { get; set; }
    public string? ForegroundColorHex { get; set; }

    [JsonIgnore]
    private SolidColorBrush? foregroundColor;

    [JsonIgnore]
    public SolidColorBrush? ForegroundColor {
        get {
            if(ForegroundColorHex is null) {
                return null;
            }

            return foregroundColor ??= BrushHelper.GetColorFromHex(ForegroundColorHex);
        }
        set {
            foregroundColor = value;
            ForegroundColorHex = value?.Color.ToString();
        }
    }
}
