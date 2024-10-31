using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Documents;
using System.Windows;

namespace HBLibrary.Logging.FlowDocumentTarget;
public class FlowDocumentJsonConverter : JsonConverter<FlowDocument> {
    public override void Write(Utf8JsonWriter writer, FlowDocument value, JsonSerializerOptions options) {
        if (value == null) {
            writer.WriteNullValue();
            return;
        }

        string xaml = SerializeFlowDocumentToXaml(value);
        writer.WriteStringValue(xaml);
    }

    public override FlowDocument? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) {
        if (reader.TokenType == JsonTokenType.Null) {
            return null;
        }

        string? xaml = reader.GetString();
        return DeserializeFlowDocumentFromXaml(xaml);
    }

    private string SerializeFlowDocumentToXaml(FlowDocument document) {
        using (MemoryStream memoryStream = new MemoryStream()) {
            var textRange = new TextRange(document.ContentStart, document.ContentEnd);
            textRange.Save(memoryStream, DataFormats.Xaml); // Save as XAML
            return Encoding.UTF8.GetString(memoryStream.ToArray());
        }
    }

    private FlowDocument DeserializeFlowDocumentFromXaml(string? xamlString) {
        var document = new FlowDocument();
        if (string.IsNullOrWhiteSpace(xamlString)) { 
            return document; 
        }

        using (MemoryStream memoryStream = new MemoryStream(Encoding.UTF8.GetBytes(xamlString))) {
            var textRange = new TextRange(document.ContentStart, document.ContentEnd);
            textRange.Load(memoryStream, DataFormats.Xaml); // Load from XAML
        }

        return document;
    }
}
