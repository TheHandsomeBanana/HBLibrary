using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
#if WINDOWS
namespace HBLibrary.Common.Security.Credentials;
public class UserCredentials {
    public required string Username { get; set; }
    public required string Salt { get; set; }
    public required string Password { get; set; }
}
#endif