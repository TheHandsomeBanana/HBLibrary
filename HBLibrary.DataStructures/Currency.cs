using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace HBLibrary.DataStructures;
public readonly struct Currency : IEquatable<Currency> {
    public string ISO4217 { get; }
    public string Name { get; }

    private Currency(string iso4217, string name) {
        ISO4217 = iso4217;
        Name = name;
    }

    public static Currency EUR => new("EUR", "Euro");
    public static Currency USD => new("USD", "US Dollar");
    public static Currency JPY => new("JPY", "Japanese Yen");
    public static Currency BGN => new("BGN", "Bulgarian Lev");
    public static Currency CZK => new("CZK", "Czech Republic Koruna");
    public static Currency DKK => new("DKK", "Danish Krone");
    public static Currency GBP => new("GBP", "British Pound Sterling");
    public static Currency HUF => new("HUF", "Hungarian Forint");
    public static Currency PLN => new("PLN", "Polish Zloty");
    public static Currency RON => new("RON", "Romanian Leu");
    public static Currency SEK => new("SEK", "Swedish Krona");
    public static Currency CHF => new("CHF", "Swiss Franc");
    public static Currency ISK => new("ISK", "Icelandic Króna");
    public static Currency NOK => new("NOK", "Norwegian Krone");
    public static Currency HRK => new("HRK", "Croatian Kuna");
    public static Currency RUB => new("RUB", "Russian Ruble");
    public static Currency TRY => new("TRY", "Turkish Lira");
    public static Currency AUD => new("AUD", "Australian Dollar");
    public static Currency BRL => new("BRL", "Brazilian Real");
    public static Currency CAD => new("CAD", "Canadian Dollar");
    public static Currency CNY => new("CNY", "Chinese Yuan");
    public static Currency HKD => new("HKD", "Hong Kong Dollar");
    public static Currency IDR => new("IDR", "Indonesian Rupiah");
    public static Currency ILS => new("ILS", "Israeli New Shekel");
    public static Currency INR => new("INR", "Indian Rupee");
    public static Currency KRW => new("KRW", "South Korean Won");
    public static Currency MXN => new("MXN", "Mexican Peso");
    public static Currency MYR => new("MYR", "Malaysian Ringgit");
    public static Currency NZD => new("NZD", "New Zealand Dollar");
    public static Currency PHP => new("PHP", "Philippine Peso");
    public static Currency SGD => new("SGD", "Singapore Dollar");
    public static Currency THB => new("THB", "Thai Baht");
    public static Currency ZAR => new("ZAR", "South African Rand");

    private static readonly Dictionary<string, Currency> _map = new[]
    {
        EUR, USD, JPY, BGN, CZK, DKK, GBP, HUF, PLN,
        RON, SEK, CHF, ISK, NOK, HRK, RUB, TRY, AUD,
        BRL, CAD, CNY, HKD, IDR, ILS, INR, KRW, MXN,
        MYR, NZD, PHP, SGD, THB, ZAR
    }.ToDictionary(c => c.ISO4217, c => c);

    public static IReadOnlyCollection<Currency> All => _map.Values;

    public override string ToString() => $"{ISO4217} – {Name}";

    public bool Equals(Currency other) => ISO4217 == other.ISO4217;

    public override bool Equals(object? obj) => obj is Currency other && Equals(other);

    public override int GetHashCode() => ISO4217.GetHashCode();

    public static bool TryParse(string code, out Currency currency)
    {
        if (code == null)
        {
            currency = default!;
            return false;
        }

        return _map.TryGetValue(code.Trim().ToUpperInvariant(), out currency);
    }

    public static bool operator ==(Currency left, Currency right) => left.Equals(right);
    public static bool operator !=(Currency left, Currency right) => !(left == right);
}
