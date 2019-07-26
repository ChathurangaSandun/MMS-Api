using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Accoon.MMS.Api.Application.UserCases.AccountActor.RegisterUser
{
    public class RegisterUserResponseHandler : INotificationHandler<RegisterUserResponse>
    {
        public RegisterUserResponseHandler()
        {

        }

        public Task Handle(RegisterUserResponse notification, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
