using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace HBLibrary.Core.MoneyConverter;
public sealed class FreeCurrencyRatesResponse {
    [JsonPropertyName("data")]
    public Dictionary<string, decimal> Data { get; set; } = [];
}
