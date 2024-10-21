using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HBLibrary.Interface.Security.Account;
public class AccountApplicationInfo {
    public required string Application { get; set; }
    public DateTime LastLogin { get; set; }
    public DateTime InitialLogin { get; set; }
}
