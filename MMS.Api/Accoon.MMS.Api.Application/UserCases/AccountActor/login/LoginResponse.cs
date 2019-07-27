using Accoon.MMS.Api.Application.Entities.Auth;
using Accoon.MMS.Api.Application.Entities.Common;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Accoon.MMS.Api.Application.UserCases.AccountActor.login
{
    public class LoginResponse :  INotification
    {
        public bool IsAuthenticated { get;  }
        public AccessToken AccessToken { get; } 
        public string RefreshToken { get; }

        public LoginResponse(bool isAuthenticated, AccessToken accessToken, string refreshToken)
        {
            this.IsAuthenticated = isAuthenticated;
            this.AccessToken = accessToken;
            this.RefreshToken = refreshToken;
        }

        public LoginResponse(bool isisAuthenticated): this(isisAuthenticated, null, null)
        {   
        }
    }
}
