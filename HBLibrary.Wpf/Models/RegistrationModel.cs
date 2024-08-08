using HBLibrary.Common.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace HBLibrary.Wpf.Models; 
public class RegistrationModel {
    public AccountType AccountType { get; set; }
    public string Username { get; set; } = "";
    public SecureString SecurePassword { get; set; } = new SecureString();
    public SecureString ConfirmSecurePassword { get; set; } = new SecureString();
    public string Email { get; set; } = "";
}
