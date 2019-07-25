using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Accoon.MMS.Api.Presenter.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : MainBaseController
    {
    }

    [Route("")]
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task Post([FromBody] AccountPostRequest request)
    {

    }


    // register user
}