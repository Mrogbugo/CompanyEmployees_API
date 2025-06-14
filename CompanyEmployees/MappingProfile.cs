﻿using AutoMapper;
using Entities.Models;
using Share.DataTransferObjects;

namespace CompanyEmployees
{
    public class MappingProfile : Profile
    { 
        public MappingProfile() 
        {
            CreateMap<Company, CompanyDto>()
                .ForCtorParam("FullAddress",
                 opt => opt.MapFrom(x => string.Join(' ', x.Address, x.Country)));

            CreateMap<Employee, EmployeeDto>();

            CreateMap<CompanyForCreationDto, Company>(); 

            CreateMap<EmployeeForCreationDto, Employee>();
            CreateMap<EmployeeForUpdateDto, Employee>().ReverseMap();
            CreateMap<CompanyForUpdateDto, Company>();
            CreateMap<UserForRegistrationDto, User>();

        }
    }
}
