using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace HBLibrary.Common.Json {
    public class DateTimeFormatConverter : JsonConverter<DateTime> {
        private readonly string format;

        public DateTimeFormatConverter(string format) {
            this.format = format;
        }

        public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) {
            return DateTime.ParseExact(reader.GetString()!, this.format, CultureInfo.InvariantCulture);
        }

        public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options) {
            if (writer is null)
                throw new ArgumentNullException(nameof(writer));

            writer.WriteStringValue(value
                .ToUniversalTime()
                .ToString(this.format, CultureInfo.InvariantCulture));
        }
    }

    public sealed class JsonDateTimeFormatAttribute : JsonConverterAttribute {
        public string Format { get; }

        public JsonDateTimeFormatAttribute(string format) {
            Format = format;
        }

        public override JsonConverter CreateConverter(Type typeToConvert) {
            return new DateTimeFormatConverter(Format);
        }
    }
}
