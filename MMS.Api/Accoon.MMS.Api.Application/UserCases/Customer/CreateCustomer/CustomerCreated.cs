using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Accoon.MMS.Api.Application.UserCases.Customer.CreateCustomer
{
    public class CustomerCreated : INotification
    {
        public Guid CustomerId { get; set; }
    }
}
