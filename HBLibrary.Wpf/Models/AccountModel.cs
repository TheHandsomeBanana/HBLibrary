using HBLibrary.Wpf.Services.AccountService;
using HBLibrary.Wpf.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HBLibrary.Wpf.Models;

public class AccountModel {
    public AccountType AccountType { get; set; }
    public required AccountDetail AccountDetail { get; set; }

}

public enum AccountType {
    Local,
    Microsoft
}