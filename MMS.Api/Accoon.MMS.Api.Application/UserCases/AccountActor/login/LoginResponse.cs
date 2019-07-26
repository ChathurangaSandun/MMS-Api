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
        public bool Success { get; set; }
        public AccessToken AccessToken { get; set; }
        public string RefreshToken { get; set; }
    }
}
