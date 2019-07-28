using Accoon.MMS.Api.Application.Interfaces.Database;
using Accoon.MMS.Api.Application.Interfaces.Repositories;
using Accoon.MMS.Api.Application.UserCases.AccountActor.CreateUser;
using Accoon.MMS.Api.Domain.Entities;
using Accoon.MMS.Api.Domain.Exceptions;
using Accoon.MMS.Api.Domain.Identity;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
namespace Accoon.MMS.Api.Application.UserCases.AccountActor.RegisterUser
{
    public class RegisterUserHandler : IRequestHandler<RegisterUserRequest, RegisterUserResponse>
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IMediator mediator;
        private readonly IDatabaseContext databaseContext;
        private readonly IUserRepository userRepository;

        public RegisterUserHandler(UserManager<AppUser> userManager, IMediator mediator, IDatabaseContext databaseContext,
            IUserRepository userRepository)
        {
            this._userManager = userManager;
            this.mediator = mediator;
            this.databaseContext = databaseContext;
            this.userRepository = userRepository;
        }

        public async Task<RegisterUserResponse> Handle(RegisterUserRequest request, CancellationToken cancellationToken)
        {
            // save app user
            var appUser = new AppUser() { Email = request.Email, UserName = request.UserName};           
            var userCreateResult = await this._userManager.CreateAsync(appUser, request.Password);
            if (!userCreateResult.Succeeded)
            {
                //TODO best error returning way
                var firstError = userCreateResult.Errors.First();
                throw new AppUserCreationFaildException(firstError.Description);
            }

            // save user 
            var user = new User()
            {
                Email = request.Email,
                FirstName = request.FirstName,
                LastName = request.LastName,
                UserName = appUser.UserName,
                IdentityId = appUser.Id,
            };
            var insertedUser = await this.userRepository.AddUserAsync(user);

            // user registraion response 
            var response = new RegisterUserResponse(appUser.Id, user.Id.ToString());
            await this.mediator.Publish(response, cancellationToken);
            return response ;
        }
    }
}
