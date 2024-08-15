using HBLibrary.Common.Account;
using System.Security;
using System.Windows.Controls;

namespace HBLibrary.Wpf.ViewModels.Login;
public abstract class LoginTriggerData {
    public abstract AccountType AccountType { get; }
    public required UserControl ControlContext { get; set; }
}

public class LocalLoginTriggerData : LoginTriggerData {
    public override AccountType AccountType => AccountType.Local;
    public string Username { get; set; } = "";
    public SecureString SecurePassword { get; set; } = new SecureString();
}

public class MicrosoftLoginTriggerData : LoginTriggerData {
    public override AccountType AccountType => AccountType.Microsoft;

}
