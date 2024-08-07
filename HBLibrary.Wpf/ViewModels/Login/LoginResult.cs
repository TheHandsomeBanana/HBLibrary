using HBLibrary.Common.Account;
using HBLibrary.Wpf.Models;
using System;
using System.Collections.Generic;
using System.IO.Packaging;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace HBLibrary.Wpf.ViewModels.Login; 
public abstract class LoginResult {
    public abstract AccountType AccountType { get; }
}

public class LocalLoginResult : LoginResult {
    public override AccountType AccountType => AccountType.Local;
    public string Username { get; set; } = "";
    public SecureString SecurePassword { get; set; } = new SecureString();

}

public class MicrosoftLoginResult : LoginResult {
    public override AccountType AccountType => AccountType.Microsoft;
    public string Username { get; set; } = "";

}
