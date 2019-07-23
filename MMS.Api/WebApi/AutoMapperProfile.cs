using AutoMapper;
using MMS.Api.BussinessLayer.Entities.EntityDtos;
using MMS.Api.Common.Pagination;
using MMS.Api.DataAccessServices.Entities.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi
{
    public class AutoMapperProfile: Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<CustomerDto, CustomerEntity>().ReverseMap();
            CreateMap<PaginationModel<CustomerEntity, Guid>, PaginationDto<CustomerDto>>();
        }
    }
}
