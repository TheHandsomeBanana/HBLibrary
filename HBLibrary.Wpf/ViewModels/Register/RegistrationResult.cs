using HBLibrary.Common.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace HBLibrary.Wpf.ViewModels.Register;

public abstract class RegistrationResult {
    public abstract AccountType AccountType { get; }
}

public class LocalRegistrationResult : RegistrationResult {
    public override AccountType AccountType => AccountType.Local;
    public string Username { get; set; } = "";
    public SecureString SecurePassword { get; set; } = new SecureString();

}

public class MicrosoftRegistrationResult : RegistrationResult {
    public override AccountType AccountType => AccountType.Microsoft;
    public string Username { get; set; } = "";

}



