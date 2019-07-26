using System;
using System.Collections.Generic;
using System.Text;

namespace Accoon.MMS.Api.Application.Interfaces.Services.Auth
{
    public interface ITokenFactory
    {
        string GenerateToken(int size = 32);
    }
}
