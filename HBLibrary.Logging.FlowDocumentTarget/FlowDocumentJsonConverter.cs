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
using HBLibrary.Interface.Logging.Statements;
using HBLibrary.Interface.Logging;

namespace HBLibrary.Logging.FlowDocumentTarget;
public class FlowDocumentJsonConverter : JsonConverter<FlowDocumentTarget> {
    public override FlowDocumentTarget? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) {
        FlowDocumentTarget target = new FlowDocumentTarget();

        if (reader.TokenType != JsonTokenType.StartArray)
            throw new JsonException("Expected an array of log messages.");

        while (reader.Read() && reader.TokenType != JsonTokenType.EndArray) {
            LogWithMetadata? logStatement = JsonSerializer.Deserialize<LogWithMetadata>(ref reader, options);

            if (logStatement is not null) {
                if (logStatement.IsSuccess) {
                    target.WriteSuccessLog(logStatement.Log);
                }
                else {
                    target.WriteLog(logStatement.Log);
                }
            }
        }

        return target;
    }

    public override void Write(Utf8JsonWriter writer, FlowDocumentTarget value, JsonSerializerOptions options) {
        writer.WriteStartArray();
        foreach (LogWithMetadata logStatement in value.Statements) {
            JsonSerializer.Serialize(writer, logStatement, options);
        }
        writer.WriteEndArray();
    }
}
