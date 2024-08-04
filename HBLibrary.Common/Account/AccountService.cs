using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HBLibrary.Common.Account;
public class AccountService : IAccountService {
    private AccountDetails currentAccount;

    public AccountDetails GetCurrentAccount() {
        return currentAccount;
    }

    void IAccountService.SetCurrentAccount(AccountDetails account) {
        currentAccount = account;
    }
}
