using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Accoon.MMS.Api.Application.UserCases.Customer.CreateCustomer
{
    public class CustomerCreatedHandler : INotificationHandler<CustomerCreated>
    {

        public CustomerCreatedHandler()
        {
        }
        public Task Handle(CustomerCreated response, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;

        }
    }
}
