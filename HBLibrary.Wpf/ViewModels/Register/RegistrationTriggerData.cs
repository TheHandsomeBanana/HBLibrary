using HBLibrary.Interface.Security.Account;
using System.Security;
using System.Windows.Controls;

namespace HBLibrary.Wpf.ViewModels.Register;

public abstract class RegistrationTriggerData {
    public abstract AccountType AccountType { get; }
    public required UserControl ControlContext { get; set; }
}

public class LocalRegistrationTriggerData : RegistrationTriggerData {
    public override AccountType AccountType => AccountType.Local;
    public string Username { get; set; } = "";
    public SecureString SecurePassword { get; set; } = new SecureString();

}

public class MicrosoftRegistrationTriggerData : RegistrationTriggerData {
    public override AccountType AccountType => AccountType.Microsoft;
    public string Username { get; set; } = "";

}



