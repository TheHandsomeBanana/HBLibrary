using HBLibrary.Common.Account;
using HBLibrary.Wpf.Models;
using System;
using System.Collections.Generic;
using System.IO.Packaging;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;
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
