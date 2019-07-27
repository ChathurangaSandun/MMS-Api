using Accoon.MMS.Api.Application.Entities.Common;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Accoon.MMS.Api.Application.UserCases.AccountActor.RegisterUser
{
    public class RegisterUserResponse:  INotification
    {        
        public string AppUserId { get;  }
        public string UserId { get;  }
        public IEnumerable<IdentityError> IdentityErrors { get; }

        public RegisterUserResponse(string appUserId, string userId)
        {
            this.AppUserId = appUserId;
            this.UserId = userId;
            this.IdentityErrors = null;
        }

        public RegisterUserResponse(IEnumerable<IdentityError> identityErrors)
        {
            this.AppUserId = null;
            this.UserId = null;
            this.IdentityErrors = identityErrors;
        }

    }
}
