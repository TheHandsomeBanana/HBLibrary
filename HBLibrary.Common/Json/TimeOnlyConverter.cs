using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace HBLibrary.Common.Json;
#if NET6_0_OR_GREATER
public class TimeOnlyConverter : JsonConverter<TimeOnly> {
    private const string TIME_FORMAT = "HH:mm:ss.fff";
    public override TimeOnly Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) {
        string timeString = reader.GetString()!;
        return TimeOnly.ParseExact(timeString, TIME_FORMAT, null);
    }

    public override void Write(Utf8JsonWriter writer, TimeOnly value, JsonSerializerOptions options) {
        writer.WriteStringValue(value.ToString(TIME_FORMAT));
    }
}
#endif
