using HBLibrary.DataStructures;
using HBLibrary.Interface.Core.MoneyConverter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Http;
using System.Net.Http.Json;

namespace HBLibrary.Core.MoneyConverter;
// https://app.freecurrencyapi.com/dashboard
public sealed class FreeCurrencyApiMoneyConverter : IMoneyConverter {
    private readonly HttpClient client;
    private readonly string apiKey;
    public FreeCurrencyApiMoneyConverter(HttpClient client, string apiKey) {
        this.client = client;
        this.apiKey = apiKey;
    }

    public async Task<Money> ConvertToAsync(Money from, Currency to) {
        if (from.Currency == to)
            return from;

        var rate = await GetRateAsync(from.Currency, to);
        return new Money(from.Amount * rate, to);
    }

    public async Task<decimal> GetRateAsync(Currency from, Currency to) {
        var url = $"https://api.freecurrencyapi.com/v1/latest?apikey={apiKey}&base_currency={from.ISO4217}&currencies={to.ISO4217}";
        var response = await client.GetFromJsonAsync<FreeCurrencyRatesResponse>(url)
                      ?? throw new Exception("Invalid API response");

        if (response.Data.TryGetValue(to.ISO4217, out decimal rate)) {
            return rate;
        }

        throw new InvalidOperationException($"Rate from {from.ISO4217} to {to.ISO4217} not found.");
    }
}
