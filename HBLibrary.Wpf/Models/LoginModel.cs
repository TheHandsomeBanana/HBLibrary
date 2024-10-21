using HBLibrary.Interface.Security.Account;
using System.Security;

namespace HBLibrary.Wpf.Models;
public class LoginModel {
    public AccountType AccountType { get; set; }
    public string Username { get; set; } = "";
    public SecureString SecurePassword { get; set; } = new SecureString();
    public string Email { get; set; } = "";

}
