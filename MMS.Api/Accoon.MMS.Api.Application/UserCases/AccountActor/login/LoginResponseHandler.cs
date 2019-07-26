using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Accoon.MMS.Api.Application.UserCases.AccountActor.login
{
    public class LoginResponseHandler :  INotificationHandler<LoginResponse>
    {
        public Task Handle(LoginResponse notification, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
