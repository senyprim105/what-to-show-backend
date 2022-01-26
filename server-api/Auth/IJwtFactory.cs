
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace server_api.Auth
{
    public interface IJwtFactory
    {
        Task<string> GenerateEncodedToken(string userName, ClaimsIdentity identity);
        ClaimsIdentity GenerateClaimsIdentity(string userName, string id, IEnumerable<string> roles)=> throw new NotImplementedException();
        Task<string> GenerateJwt(ClaimsIdentity identity, string userName);
    }
}
