using Accoon.MMS.Api.Application.Interfaces.Automapper;
using AutoMapper;
using System;

namespace Accoon.MMS.Api.Application.UserCases.Customer.GetCustomerList
{
    public class CustomerDetailModel : IHaveCustomMapping
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }

        public void CreateMappings(Profile configuration)
        {
            configuration.CreateMap<Accoon.MMS.Api.Domain.Entities.Customer, CustomerDetailModel>()
               .ForMember(cDTO => cDTO.Id, opt => opt.MapFrom(c => c.Id))
               .ForMember(cDTO => cDTO.Name, opt => opt.MapFrom(c => c.Name))
               .ForMember(cDTO => cDTO.Age, opt => opt.MapFrom(c => c.Age));
        }
    }
}