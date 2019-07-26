using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Accoon.MMS.Api.Application.UserCases.AccountActor.login;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Accoon.MMS.Api.Presenter.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : MainBaseController
    {
        [HttpPost("login")]
        public async Task<ActionResult<LoginResponse>> Login([FromBody] LoginRequest request)
        {
            var result = await Mediator.Send(request);
            return result ;
        }
    }

    // login

    // refresh token

}