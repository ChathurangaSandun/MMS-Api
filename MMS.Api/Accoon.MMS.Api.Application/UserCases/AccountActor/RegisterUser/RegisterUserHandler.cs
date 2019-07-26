using Accoon.MMS.Api.Application.Interfaces.Database;
using Accoon.MMS.Api.Application.UserCases.AccountActor.CreateUser;
using Accoon.MMS.Api.Domain.Entities;
using Accoon.MMS.Api.Domain.Identity;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Accoon.MMS.Api.Application.UserCases.AccountActor.RegisterUser
{
    public class RegisterUserHandler : IRequestHandler<RegisterUserRequest, RegisterUserResponse>
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IMediator mediator;
        private readonly IDatabaseContext databaseContext;

        public RegisterUserHandler(UserManager<AppUser> userManager, IMediator mediator, IDatabaseContext databaseContext)
        {
            this._userManager = userManager;
            this.mediator = mediator;
            this.databaseContext = databaseContext;
        }

        public async Task<RegisterUserResponse> Handle(RegisterUserRequest request, CancellationToken cancellationToken)
        {
            var userResponse = new RegisterUserResponse() { AppUserId = null, UserId = null, Success = false };
            var appUser = new AppUser() { Email = request.Email, UserName = request.UserName};
            var userCreateResult = await this._userManager.CreateAsync(appUser, request.Password);
            if (!userCreateResult.Succeeded)
            {
                userResponse.AppUserId = appUser.Id;
                await this.mediator.Publish(userResponse, cancellationToken);
                return userResponse;
            }

            var user = new User()
            {
                Email = request.Email,
                FirstName = request.FirstName,
                LastName = request.LastName,
                UserName = appUser.UserName,
                IdentityId = appUser.Id,
            };
            await this.databaseContext.Users.AddAsync(user);
            await this.databaseContext.SaveChangesAsync(cancellationToken);
            userResponse.AppUserId = appUser.Id;
            userResponse.UserId = user.Id.ToString();
            userResponse.Success = true;
            await this.mediator.Publish(userResponse, cancellationToken);
            return userResponse;
        }
    }
}
