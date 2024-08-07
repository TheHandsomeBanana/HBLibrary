using HBLibrary.Common.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HBLibrary.Wpf.Models; 
public class MicrosoftLoginModel {
    public AccountType AccountType => AccountType.Microsoft;
    public string Email { get; set; }
}
