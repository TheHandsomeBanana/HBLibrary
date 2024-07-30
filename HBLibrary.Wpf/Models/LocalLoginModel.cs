using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace HBLibrary.Wpf.Models;
public class LocalLoginModel {
    public string Username { get; set; } = "";
    public SecureString SecurePassword { get; set; } = new SecureString();
}
