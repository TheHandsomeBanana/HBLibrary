using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HBLibrary.Wpf.Models; 
public abstract class LoginModel {
    public abstract AccountType AccountType { get; }
}
