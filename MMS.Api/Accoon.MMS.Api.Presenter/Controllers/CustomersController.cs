using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Accoon.MMS.Api.Application.UserCases.Customer.CreateCustomer;
using Accoon.MMS.Api.Application.UserCases.Customer.GetCustomer;
using Accoon.MMS.Api.Application.UserCases.Customer.GetCustomerList;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Accoon.MMS.Api.Presenter.Controllers
{
    [Authorize(Policy = "ApiUser")]
    public class CustomersController : MainBaseController
    {
        private readonly ILogger<CustomersController> logger;

        public CustomersController(ILogger<CustomersController> logger)
        {
            this.logger = logger;
        }

        [Route("")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create([FromBody] CreateCustomerCommand createCustomerCommand)
        {            
            var customer = await Mediator.Send(createCustomerCommand);
            return CreatedAtAction(nameof(Get), new { id = customer.CustomerId }, null);
        }

        [Route("{id:guid}")]
        [HttpGet]
        [ProducesResponseType(typeof(CustomerModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<CustomerModel>> Get([FromRoute] Guid id)
        {
            var customer = await Mediator.Send(new GetCustomerQuery() { Id = id });
            if (customer == null)
            {
                return NotFound();
            }

            return Ok(customer);
        }

        [Route("")]
        [HttpGet]
        [ProducesResponseType(typeof(CustomerListViewModel), StatusCodes.Status200OK)]
        public async Task<ActionResult<CustomerListViewModel>> Get()
        {
            logger.LogInformation("start : ");
            var customerListModel = await Mediator.Send(new GetCustomersListQuery());
            return Ok(customerListModel);
        }
    }
}