using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HBLibrary.Common.Authentication;
public interface IAuthenticationService<TAuthResult, TAuthCredentials> where TAuthResult : IAuthResult where TAuthCredentials : IAuthCredentials {
    public Task<TAuthResult> Authenticate(TAuthCredentials authCredentials);
}
