﻿using AutoMapper;
using Contracts;
using Entities;
using Entities.Exceptions;
using Entities.LinkModels;
using Entities.Models;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Service.Contracts;
using Share.DataTransferObjects;
using Share.RequestFeatures;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public sealed class EmployeeService : IEmployeeService
    {
        private readonly IRepositoryManager _repository;
        private readonly ILoggerManager _logger;
        private readonly IMapper _mapper;
       // private readonly IDataShaper<EmployeeDto> _dataShaper;
        private readonly IEmployeeLinks _employeeLinks;

        public EmployeeService(IRepositoryManager repository, ILoggerManager logger, IMapper mapper, IEmployeeLinks employeeLinks)
        {
            _repository = repository;
            _logger = logger;
            _mapper = mapper;
          //  _dataShaper = dataShaper;
            _employeeLinks = employeeLinks;
        }

        public async Task<(LinkResponse linkResponse, MetaData metaData)> GetEmployeesAsync (Guid companyId, LinkParameters linkParameters, bool trackChanges)
        {
            //var company = await _repository.Company.GetCompanyAsync(companyId, trackChanges);
            //if (company is null) 
            //    throw new CompanyNotFoundException(companyId);  

            if (!linkParameters.EmployeeParameters.ValidAgeRange)
                throw new MaxAgeRangeBadRequestException(); 

            await CheckIfCompanyExist(companyId, trackChanges);

            var employeesWithMetaData = await _repository.Employee.GetEmployeesAsync(companyId, linkParameters.EmployeeParameters, trackChanges);
            var employeesDto = _mapper.Map<IEnumerable<EmployeeDto>>(employeesWithMetaData);

            var links = _employeeLinks.TryGenerateLinks(employeesDto,linkParameters.EmployeeParameters.Fields,companyId, linkParameters.Context);

            return (linkResponse: links, metaData: employeesWithMetaData.MetaData);


        }
           
        public async Task <EmployeeDto> GetEmployeeAsync(Guid companyId, Guid id, bool trackChanges)
        {
            var company = await _repository.Company.GetCompanyAsync(companyId, trackChanges);
            if(company is null)
                throw new CompanyNotFoundException(companyId);

            var employeeDb = await _repository.Employee.GetEmployeeAsync(companyId, id, trackChanges);
            if (employeeDb is null)
                throw new EmployeeNotFoundException(id); 
            var employee = _mapper.Map<EmployeeDto>(employeeDb);
            return employee;
        }

        public async Task<EmployeeDto> CreateEmployeeForCompanyAsync(Guid companyId, EmployeeForCreationDto employeeForCreationDto, bool trackChanges)
        {
            //var company = await _repository.Company.GetCompanyAsync(companyId, trackChanges); 
            //if(company is null)
            //    throw new CompanyNotFoundException(companyId);  
            await CheckIfCompanyExist(companyId, trackChanges); 

            var employeeEntity = _mapper.Map<Employee>(employeeForCreationDto);
            _repository.Employee.CreateEmployeeForCompany(companyId, employeeEntity);
           await _repository.SaveAsync(); 

            var employeeToReturn = _mapper.Map<EmployeeDto>(employeeEntity);

            return employeeToReturn;
        }  

        public async Task DeleteEmployeeForCompanyAsync(Guid companyId, Guid id, bool trackChanges)
        {
            //var company = await _repository.Company.GetCompanyAsync(companyId, trackChanges);
            //if (company is null)
            //    throw new CompanyNotFoundException(companyId);   
            await CheckIfCompanyExist(companyId, trackChanges); 
            var employeeForCompany = await _repository.Employee.GetEmployeeAsync(companyId, id, trackChanges);
            if (employeeForCompany is null)
                throw new EmployeeNotFoundException(id);
            _repository.Employee.DeleteEmployee(employeeForCompany);
          await  _repository.SaveAsync();
        } 

        public async Task UpdateEmployeeForCompanyAsync(Guid companyId, Guid id,EmployeeForUpdateDto employeeForUpdate, bool compTrackChanges, bool empTrackChanges)
        {
            //var company = await _repository.Company.GetCompanyAsync(companyId, compTrackChanges);
            //if (company is null)
            //    throw new CompanyNotFoundException(companyId);   
            await CheckIfCompanyExist(companyId, compTrackChanges);
            //var employeeDb = _repository.Employee.GetEmployeeAsync(companyId, id, empTrackChanges);
            ////if (employeeDb is null)
            ////    throw new EmployeeNotFoundException(id); 
            var employeeDb = await GetEmployeeForCompanyAndCheckIfItExists(companyId,id,empTrackChanges);
            _mapper.Map(employeeForUpdate, employeeDb);
          await  _repository.SaveAsync();
        } 


       public async Task<(EmployeeForUpdateDto employeeToPatch, Employee employeeEntity)>
              GetEmployeeForPatchAsync(Guid companyId, Guid id, bool compTrackChanges, bool empTrackChanges) 
       {
            //var company = await _repository.Company.GetCompanyAsync(companyId, compTrackChanges); 
            //if (company is null) 
            //         throw new CompanyNotFoundException(companyId);  
            await CheckIfCompanyExist(companyId, compTrackChanges);
 
              //var employeeEntity = await _repository.Employee.GetEmployeeAsync(companyId, id, empTrackChanges); 
              //if (employeeEntity is null) 
              //       throw new EmployeeNotFoundException(companyId);  
              var employeeDb = await GetEmployeeForCompanyAndCheckIfItExists(companyId, id, empTrackChanges);
  
              var employeeToPatch = _mapper.Map<EmployeeForUpdateDto>(employeeDb); 
             return (employeeToPatch : employeeToPatch,employeeEntity : employeeDb); 
       } 
 
      public async Task SaveChangesForPatchAsync(EmployeeForUpdateDto employeeToPatch, Employee employeeEntity) 
      { 
         _mapper.Map(employeeToPatch, employeeEntity); 
        await _repository.SaveAsync(); 
      } 

     private async Task CheckIfCompanyExist(Guid companyId,bool trackChanges)
     {
        var company = await _repository.Company.GetCompanyAsync(companyId, trackChanges);
            if(company is null)
                throw new CompanyNotFoundException(companyId);
     } 
     
     private async Task<Employee> GetEmployeeForCompanyAndCheckIfItExists(Guid companyId, Guid id, bool trackChanges)
     {
        var employeeDb = await _repository.Employee.GetEmployeeAsync(companyId, id, trackChanges);
            if(employeeDb is null)
                throw new EmployeeNotFoundException(companyId);
            return employeeDb;
     }


    }
}
