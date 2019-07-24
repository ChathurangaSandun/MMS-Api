using Accoon.MMS.Api.Application.Interfaces.Database;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Accoon.MMS.Api.Application.UserCases.Customer.GetCustomerList
{
    public class GetCustomersListQueryHandler : IRequestHandler<GetCustomersListQuery, CustomerListViewModel>
    {
        private readonly IDatabaseContext cqrscaDbContext;
        private readonly IMapper mapper;

        public GetCustomersListQueryHandler(IDatabaseContext cqrscaDbContext, IMapper mapper)
        {
            this.cqrscaDbContext = cqrscaDbContext;
            this.mapper = mapper;
        }

        public async Task<CustomerListViewModel> Handle(GetCustomersListQuery request, CancellationToken cancellationToken)
        {
            // get from database
            var customerList = await this.cqrscaDbContext.Customers
                .ProjectTo<CustomerDetailModel>(this.mapper.ConfigurationProvider).ToListAsync<CustomerDetailModel>(cancellationToken);

            var model = new CustomerListViewModel()
            {
                Customers = customerList
            };

            return model;
        }
    }
}
