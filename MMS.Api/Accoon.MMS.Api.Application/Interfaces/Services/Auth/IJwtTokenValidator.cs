using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;

namespace Accoon.MMS.Api.Application.Interfaces.Services.Auth
{
    public interface IJwtTokenValidator
    {
        ClaimsPrincipal GetPrincipalFromToken(string token, string signingKey);
    }
}
