using HBLibrary.Interface.Security.Keys;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace HBLibrary.Security.Rsa;
public class RsaKeyConverter : JsonConverter<RsaKey> {
    public override RsaKey Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) {
        JsonDocument jsonDoc = JsonDocument.ParseValue(ref reader);

        string? keyBase64 = jsonDoc.RootElement.GetProperty("Key").GetString();
        int keySize = jsonDoc.RootElement.GetProperty("KeySize").GetInt32();
        bool isPublic = jsonDoc.RootElement.GetProperty("IsPublic").GetBoolean();

        return new RsaKey(
            keyBase64 != null ? Convert.FromBase64String(keyBase64) : [],
            keySize,
            isPublic);
    }

    public override void Write(Utf8JsonWriter writer, RsaKey value, JsonSerializerOptions options) {
        writer.WriteStartObject();
        writer.WriteString("Key", Convert.ToBase64String(value.Key));
        writer.WriteNumber("KeySize", value.KeySize);
        writer.WriteBoolean("IsPublic", value.IsPublic);
        writer.WriteString("Name", value.Name);
        writer.WriteEndObject();
    }
}