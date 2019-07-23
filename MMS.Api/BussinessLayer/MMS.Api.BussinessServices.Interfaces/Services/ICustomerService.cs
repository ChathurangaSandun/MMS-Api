using MMS.Api.BussinessLayer.Entities.EntityDtos;
using MMS.Api.Common.Pagination;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MMS.Api.BussinessServices.Interfaces.Services
{
    public interface ICustomerService
    {        
        Task<Guid> SaveCustomerAsync(CustomerDto customer);
        Task<CustomerDto> GetCustomerByIdAsync(Guid id);
        PaginationDto<CustomerDto> GetCustomers(int page = 1, int size = 10);
    }
}
