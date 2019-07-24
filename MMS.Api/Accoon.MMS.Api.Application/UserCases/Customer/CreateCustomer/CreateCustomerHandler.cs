using Accoon.MMS.Api.Application.Interfaces.Database;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Accoon.MMS.Api.Domain.Entities;

namespace Accoon.MMS.Api.Application.UserCases.Customer.CreateCustomer
{
    public class CreateCustomerHandler : IRequestHandler<CreateCustomerCommand, CustomerCreated>
    {
        private readonly IMediator mediator;
        private readonly IDatabaseContext cqrscaDbContext;

        public CreateCustomerHandler(IMediator mediator, IDatabaseContext context)
        {
            this.mediator = mediator;
            this.cqrscaDbContext = context;
        }

        public async Task<CustomerCreated> Handle(CreateCustomerCommand request, CancellationToken cancellationToken)
        {
            var entity = new Accoon.MMS.Api.Domain.Entities.Customer()
            {
                Id = Guid.NewGuid(),
                Name = request.Name,
                Age = request.Age
            };
            //insert customer to database
            this.cqrscaDbContext.Customers.Add(entity);

            await this.cqrscaDbContext.SaveChangesAsync(cancellationToken);

            var newcustomer = new CustomerCreated { CustomerId = entity.Id };
            await this.mediator.Publish(newcustomer, cancellationToken);

            return newcustomer;
        }
    }
}
