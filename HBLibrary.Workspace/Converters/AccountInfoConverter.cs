using HBLibrary.Interface.Security.Account;
using HBLibrary.Security.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace HBLibrary.Workspace.Converters;
public class AccountInfoConverter : JsonConverter<IAccountInfo> {
    public override IAccountInfo? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) {
        return JsonSerializer.Deserialize<AccountInfo>(ref reader, options);
    }

    public override void Write(Utf8JsonWriter writer, IAccountInfo value, JsonSerializerOptions options) {
        JsonSerializer.Serialize(writer, (AccountInfo)value, options);
    }
}
