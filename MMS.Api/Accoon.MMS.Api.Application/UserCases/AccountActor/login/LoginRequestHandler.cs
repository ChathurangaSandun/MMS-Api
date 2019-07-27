using Accoon.MMS.Api.Application.Interfaces.Database;
using Accoon.MMS.Api.Application.Interfaces.Repositories;
using Accoon.MMS.Api.Application.Interfaces.Services.Auth;
using Accoon.MMS.Api.Application.UserCases.AccountActor.RegisterUser;
using Accoon.MMS.Api.Domain.Entities;
using Accoon.MMS.Api.Domain.Exceptions;
using Accoon.MMS.Api.Domain.Identity;
using AutoMapper;
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
        private readonly IRefreshTokenRepository refreshTokenRepository;
        private readonly IMapper mapper;

        public LoginRequestHandler(UserManager<AppUser> userManager, ITokenFactory tokenFactory, IDatabaseContext databaseContext, IMediator mediator, IJwtFactory jwtFactory,
            IRefreshTokenRepository refreshTokenRepository,
            IMapper mapper)
        {
            this.userManager = userManager;
            this.tokenFactory = tokenFactory;
            this.databaseContext = databaseContext;
            this.mediator = mediator;
            this.jwtFactory = jwtFactory;
            this.refreshTokenRepository= refreshTokenRepository;
            this.mapper = mapper;
        }

        public async Task<LoginResponse> Handle(LoginRequest request, CancellationToken cancellationToken)
        {
            // get app user
            var appUser = await this.userManager.FindByNameAsync(request.UserName);            
            if (appUser == null)
            {
                throw new UserNotFoundException($"Cannot find user { request.UserName }");
            }
            
            // validate password
            if (!await userManager.CheckPasswordAsync(appUser, request.Password))
            {
                throw new InvalidUsernamePasswordException("Invalid username and password. Please try again.");
            }

            // generate refresh token
            var refreshToken = tokenFactory.GenerateToken();

            // save refresh token
            await this.refreshTokenRepository.AddRefreshTokenAsync(new RefreshToken(refreshToken, DateTime.UtcNow.AddDays(5), appUser.Id.ToString(), null));
           
            // create the login response and publish
            var response = new LoginResponse(true, await jwtFactory.GenerateEncodedToken(appUser.Id, appUser.UserName), refreshToken);
            await this.mediator.Publish(response, cancellationToken);
            return response;
        }
    }
}

