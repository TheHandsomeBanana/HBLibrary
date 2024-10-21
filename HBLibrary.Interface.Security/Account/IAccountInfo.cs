using HBLibrary.DataStructures;
using HBLibrary.Interface.Security.Keys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HBLibrary.Interface.Security.Account;
public interface IAccountInfo {
    public AccountType AccountType { get; set; }
    public string AccountId { get; set; }
    public string Username { get; set; }
    public DateTime CreatedOn { get; set; }
    public DateTime ModifiedOn { get; set; }
    public List<AccountApplicationInfo> Applications { get; set; }


    public Task<Result<RsaKey>> GetPublicKeyAsync();
    public Result<RsaKey> GetPublicKey();
}


