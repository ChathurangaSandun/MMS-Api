using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Accoon.MMS.Api.Application.UserCases.AccountActor.CreateUser;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Accoon.MMS.Api.Presenter.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : MainBaseController
    {       

        [Route("")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Post([FromBody] RegisterUserRequest request)
        {
            var userResponse = await this.Mediator.Send(request);
            return CreatedAtAction(nameof(this.Post), new { id = userResponse.UserId}, null);
        }
    }
}