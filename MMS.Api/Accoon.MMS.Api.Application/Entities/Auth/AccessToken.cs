using System;
using System.Collections.Generic;
using System.Text;

namespace Accoon.MMS.Api.Application.Entities.Auth
{
    public sealed class AccessToken
    {
        public string Token { get; set; }
        public int ExpiresIn { get; set; }

        public AccessToken(string token, int expiresIn)
        {
            Token = token;
            ExpiresIn = expiresIn;
        }
    }
}
