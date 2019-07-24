using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Accoon.MMS.Api.Application.UserCases.Customer.GetCustomerList
{
    public class GetCustomersListQuery : IRequest<CustomerListViewModel>
    {
    }
}
