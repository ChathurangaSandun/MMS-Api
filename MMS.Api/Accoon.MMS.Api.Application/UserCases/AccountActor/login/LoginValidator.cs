using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Accoon.MMS.Api.Application.UserCases.AccountActor.login
{
    public class LoginValidator: AbstractValidator<LoginRequest>
    {
        public LoginValidator()
        {
            RuleFor(x => x.UserName).NotNull().NotEmpty()
                .WithMessage("Usernme is required");
            RuleFor(x => x.Password).NotNull().NotEmpty()
                .WithMessage("Password is required");
        }
    }
}
