using Accoon.MMS.Api.Application.Entities.Common;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Accoon.MMS.Api.Application.UserCases.AccountActor.RegisterUser
{
    public class RegisterUserResponse: BaseResponse, INotification
    {
        public string AppUserId { get; set; }
        public string UserId { get; set; }
    }
}
