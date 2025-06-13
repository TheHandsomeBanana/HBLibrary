using HBLibrary.DataStructures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HBLibrary.Interface.Core.MoneyConverter;
public interface IMoneyConverter {
    public Task<Money> ConvertToAsync(Money from, Currency to);
    public Task<decimal> GetRateAsync(Currency from, Currency to);
}
