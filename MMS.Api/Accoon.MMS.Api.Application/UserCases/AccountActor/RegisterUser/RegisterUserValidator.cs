using Accoon.MMS.Api.Application.UserCases.AccountActor.CreateUser;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Accoon.MMS.Api.Application.UserCases.AccountActor.RegisterUser
{
    public class RegisterUserValidator: AbstractValidator<RegisterUserRequest>
    {
        public RegisterUserValidator()
        {
            RuleFor(x => x.FirstName).NotNull().WithMessage("First Name is required");
        }
    }
}
