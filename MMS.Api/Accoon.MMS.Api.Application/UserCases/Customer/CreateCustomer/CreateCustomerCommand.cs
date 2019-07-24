using MediatR;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Accoon.MMS.Api.Application.UserCases.Customer.CreateCustomer
{
    public class CreateCustomerCommand : IRequest<CustomerCreated>
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
    }
}

