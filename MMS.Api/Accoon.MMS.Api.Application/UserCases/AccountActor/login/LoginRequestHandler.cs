using Accoon.MMS.Api.Application.Interfaces.Database;
using Accoon.MMS.Api.Application.Interfaces.Services.Auth;
using Accoon.MMS.Api.Application.UserCases.AccountActor.RegisterUser;
using Accoon.MMS.Api.Domain.Identity;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Accoon.MMS.Api.Application.UserCases.AccountActor.login
{
    public class LoginRequestHandler : IRequestHandler<LoginRequest, LoginResponse>
    {
        private readonly UserManager<AppUser> userManager;
        private readonly ITokenFactory tokenFactory;
        private readonly IDatabaseContext databaseContext;
        private readonly IMediator mediator;
        private readonly IJwtFactory jwtFactory;


        public LoginRequestHandler(UserManager<AppUser> userManager, ITokenFactory tokenFactory, IDatabaseContext databaseContext, IMediator mediator, IJwtFactory jwtFactory)
        {
            this.userManager = userManager;
            this.tokenFactory = tokenFactory;
            this.databaseContext = databaseContext;
            this.mediator = mediator;
            this.jwtFactory = jwtFactory;
        }


        public async Task<LoginResponse> Handle(LoginRequest request, CancellationToken cancellationToken)
        {
            var loginResponse = new LoginResponse() { AccessToken = null, RefreshToken = null, Success = false };

            // get user
            var user = await this.userManager.FindByNameAsync(request.UserName);
            if (user == null)
            {
                loginResponse.Success = false;
                return loginResponse;
            }

            if (!await userManager.CheckPasswordAsync(user, request.Password))
            {
                loginResponse.Success = false;
                return loginResponse;
            }

            // generate refresh token
            var refreshToken = tokenFactory.GenerateToken();
            await this.databaseContext.RefreshTokens.AddAsync(new Domain.Entities.RefreshToken()
            {
                Token = refreshToken,
                UserId = user.Id,
                RemoteIpAddress = null,
                Expires = DateTime.UtcNow.AddDays(5)

            });

            await this.databaseContext.SaveChangesAsync(cancellationToken);

            loginResponse.AccessToken = await jwtFactory.GenerateEncodedToken(user.Id, user.UserName);
            loginResponse.RefreshToken = refreshToken;
            loginResponse.Success = true;

            await this.mediator.Publish(loginResponse, cancellationToken);
            return loginResponse;
        }


    }
}

