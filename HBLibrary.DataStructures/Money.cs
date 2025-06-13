using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HBLibrary.DataStructures;
public readonly struct Money : IEquatable<Money>, IComparable<Money> {
    public decimal Amount { get; }
    public Currency Currency { get; }

    public Money(decimal amount, Currency currency) {
        Amount = amount;
        Currency = currency;
    }

    public override string ToString() {
        return $"{Amount:N2} {Currency.ISO4217}";
    }

    public string ToString(IFormatProvider formatProvider) {
        return string.Format(formatProvider, "{0:N2} {1}", Amount, Currency.ISO4217);
    }

    public bool Equals(Money other) {
        return Currency.Equals(other.Currency) && Amount == other.Amount;
    }

    public override bool Equals(object? obj) {
        return obj is Money other && Equals(other);
    }

    public override int GetHashCode() {
        return HBHashCode.Combine(Amount, Currency);
    }

    public int CompareTo(Money other) {
        if (!Currency.Equals(other.Currency)) {
            throw new InvalidOperationException("Cannot compare Money values with different currencies.");
        }

        return Amount.CompareTo(other.Amount);
    }

    public static bool operator ==(Money a, Money b) => a.Equals(b);
    public static bool operator !=(Money a, Money b) => !a.Equals(b);

    public static Money operator +(Money a, Money b) {
        EnsureSameCurrency(a, b);
        return new Money(a.Amount + b.Amount, a.Currency);
    }

    public static Money operator -(Money a, Money b) {
        EnsureSameCurrency(a, b);
        return new Money(a.Amount - b.Amount, a.Currency);
    }

    public static Money operator *(Money a, decimal multiplier) {
        return new Money(a.Amount * multiplier, a.Currency);
    }

    public static Money operator /(Money a, decimal divisor) {
        return new Money(a.Amount / divisor, a.Currency);
    }

    private static void EnsureSameCurrency(Money a, Money b) {
        if (a.Currency != b.Currency) {
            throw new InvalidOperationException("Cannot operate on Money values with different currencies.");
        }
    }
}
