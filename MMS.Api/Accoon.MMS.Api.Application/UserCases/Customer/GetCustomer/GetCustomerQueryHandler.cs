using Accoon.MMS.Api.Application.Interfaces.Database;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Accoon.MMS.Api.Application.UserCases.Customer.GetCustomer
{
    public class GetCustomerQueryHandler : IRequestHandler<GetCustomerQuery, CustomerModel>
    {
        private readonly IDatabaseContext cqrscaDbContext;
        private readonly IMapper mapper;

        public GetCustomerQueryHandler(IDatabaseContext cqrscaDbContext, IMapper mapper)
        {
            this.cqrscaDbContext = cqrscaDbContext;
            this.mapper = mapper;
        }

        public async Task<CustomerModel> Handle(GetCustomerQuery request, CancellationToken cancellationToken)
        {
            var customer = await this.cqrscaDbContext.Customers.FirstAsync(o => o.Id.Equals(request.Id));
            var getCustomer = this.mapper.Map<CustomerModel>(customer);
            return getCustomer;
        }
    }
}
