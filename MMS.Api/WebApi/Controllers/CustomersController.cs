using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MMS.Api.BussinessLayer.Entities.EntityDtos;
using MMS.Api.BussinessServices.Interfaces.Services;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        [Route("")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Save([FromServices] ICustomerService customerService, [FromBody] CustomerDto customerDto)
        {
            var id = await customerService.SaveCustomerAsync(customerDto);
            return CreatedAtAction(nameof(GetById), new { id = id }, null);
        }

        [Route("{id:guid}")]
        [HttpGet]
        [ProducesResponseType(typeof(CustomerDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<CustomerDto>> GetById([FromServices] ICustomerService customerService, Guid id)
        {
            var customer = await customerService.GetCustomerByIdAsync(id);
            if (customer == null)
            {
                return NotFound();
            }

            return customer;
        }
    }
}