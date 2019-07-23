using AutoMapper;
using MMS.Api.BussinessLayer.Entities.EntityDtos;
using MMS.Api.BussinessServices.Interfaces.Services;
using MMS.Api.Common.Concretes;
using MMS.Api.Common.Interfaces;
using MMS.Api.Common.Pagination;
using MMS.Api.DataAccessServices.Entities;
using MMS.Api.DataAccessServices.Entities.Entities;
using MMS.Api.DataAccessServices.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MMS.Api.BussinessServices.Concrets.Services
{
    public class CustomerService : ServiceBase, ICustomerService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork<MMSDbContext> _unitOfWork;
        private readonly ICustomerRepository _customerRepository;

        public CustomerService(IMapper mapper, IUnitOfWork<MMSDbContext> unitOfWork, ICustomerRepository customerRepository)
        {
            this._mapper = mapper;
            this._unitOfWork = unitOfWork;
            this._customerRepository = customerRepository;
        }

        public List<CustomerDto> GetCustomers()
        {
            var customers = this._customerRepository.GetAll().ToList();
            var result = this._mapper.Map<List<CustomerDto>>(customers);
            return result;
        }

        public async Task<CustomerDto> GetCustomerByIdAsync(Guid id)
        {
            var customerEntity = await this._customerRepository.GetAsync(id);
            var customerDto = this._mapper.Map<CustomerDto>(customerEntity);
            return customerDto;
        }
            
        public async Task<Guid> SaveCustomerAsync(CustomerDto customer)
        {
            var customerEntity = this._mapper.Map<CustomerEntity>(customer);
            var newCustomer = await this._customerRepository.InsertAsync(customerEntity);
            this._unitOfWork.Commit();
            return newCustomer.Id;
        }

        public PaginationDto<CustomerDto> GetCustomers(int page, int size)
        {
            var customerPagenation = this._customerRepository.GetPaginationAsync(page, size);
            var pagination = this._mapper.Map<PaginationDto<CustomerDto>>(customerPagenation);
            return pagination;
        }
    }
}
