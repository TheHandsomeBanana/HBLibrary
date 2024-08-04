using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HBLibrary.Common.Account;
public interface IAccountService {
    public AccountDetails GetCurrentAccount();
    internal void SetCurrentAccount(AccountDetails account);
}
