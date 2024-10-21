using HBLibrary.DataStructures;
using HBLibrary.Interface.Security.Keys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace HBLibrary.Interface.Security.Account;
public interface IAccount {
    public AccountType AccountType { get; }
    public string Application { get; set; }
    public string Username { get; set; }
    public RsaKey PublicKey { get; set; }
    public string Salt { get; set; }
    public SecureString SupportKey { get; set; }
    public string AccountId { get; }

    public Task<Result<RsaKey>> GetPrivateKeyAsync();
    public Result<RsaKey> GetPrivateKey();
    public IAccountInfo CreateNewAccountInfo();
}

public enum AccountType {
    Local,
    Microsoft
}
