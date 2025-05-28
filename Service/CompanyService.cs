using AutoMapper;
using Contracts;
using Entities.Exceptions;
using Entities.Models;
using Entities.Responses;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Identity.Client;
using Service.Contracts;
using Share.DataTransferObjects;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public sealed class CompanyService : ICompanyService
    {  
        private readonly IRepositoryManager _repository;
        private readonly ILoggerManager _logger;
        private readonly IMapper _mapper; 

        public CompanyService(IRepositoryManager repository, ILoggerManager logger, IMapper mapper)
        {
            _repository = repository;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<IEnumerable<CompanyDto>> GetAllCompaniesAsync(bool trackChanges)
        {
           
    
                var companies = await _repository.Company.GetAllCompaniesAsync(trackChanges); 
                var companiesDTO = _mapper.Map<IEnumerable<CompanyDto>>(companies);
                return companiesDTO;
          
        }

        public async Task<CompanyDto> GetCompanyAsync(Guid id, bool trackChanges)
        {
          // var company = await _repository.Company.GetCompanyAsync(id, trackChanges); 
            // check if the company is null
            //if(company is null)
            //{
            //    throw new CompanyNotFoundException(id);
            //} 

            var company = await GetCompanyAndCheckIfItExists(id, trackChanges);
            var companyDto = _mapper.Map<CompanyDto>(company); 
            return companyDto;
        } 

        public async Task<CompanyDto> CreateCompanyAsync(CompanyForCreationDto company)
        {
            var compnayEnitity = _mapper.Map<Company>(company);
            _repository.Company.CreateCompany(compnayEnitity);
             await _repository.SaveAsync(); 
            var companyToReturn = _mapper.Map<CompanyDto>(compnayEnitity);

            return companyToReturn;
        }

        public async Task<IEnumerable<CompanyDto>> GetByIdsAsync(IEnumerable<Guid> ids, bool trackChanges)
        {
            if (ids is null)
                throw new IdParametersBadRequestException(); 
            var CompanyEntities  = await _repository.Company.GetByIdsAsync(ids, trackChanges);
            if (ids.Count() != CompanyEntities.Count())
                throw new CollectionByIdsBadRequestException(); 
            var companiesToReturn = _mapper.Map<IEnumerable<CompanyDto>>(CompanyEntities);
            return companiesToReturn;
        } 

        public async Task<(IEnumerable<CompanyDto> companies, string ids)> CreateCompanyCollectionAsync
        
               (IEnumerable<CompanyForCreationDto> companyCollection)
        {
            if (companyCollection is null)
                throw new CompanyCollectionBadRequest();
            var companyEntities = _mapper.Map<IEnumerable<Company>>(companyCollection); 
            foreach (var company in companyEntities)
            {
                _repository.Company.CreateCompany(company); 
            }
           await _repository.SaveAsync();

            var companyCollectionReturn = _mapper.Map<IEnumerable<CompanyDto>>(companyEntities);
            var ids = string.Join(",", companyCollectionReturn.Select(c=>c.Id));
            return (companies:companyCollectionReturn, ids:ids);
        }  

        public async Task DeleteCompanyAsync(Guid companyId,bool trackChanges)
        {
            //var company = await _repository.Company.GetCompanyAsync(companyId, trackChanges);
            //if (company is null)
            //    throw new CompanyNotFoundException(companyId); 
            var company = await GetCompanyAndCheckIfItExists(companyId, trackChanges);
            _repository.Company.DeleteCompany(company);
          await  _repository.SaveAsync();
        }  

        public async Task UpdateCompanyAsync(Guid companyId, CompanyForUpdateDto companyForUpdate, bool trackChanges)
        {
            //var companyEntity = await _repository.Company.GetCompanyAsync(companyId, trackChanges);
            //if (companyEntity is null)
            //    throw new CompanyNotFoundException(companyId); 
            var company = await GetCompanyAndCheckIfItExists(companyId,trackChanges);
            //_mapper.Map(companyForUpdate, companyEntity);
            _mapper.Map(companyForUpdate, company);
           await _repository.SaveAsync();
        }


        //Refactoring Serice Layer to Avoid Repetition codes By Using Of GetCompanyAndCheckIfItExists Mehtod
        private async Task<Company> GetCompanyAndCheckIfItExists(Guid id, bool trackChanges) 
        {
            var company = await _repository.Company.GetCompanyAsync(id, trackChanges);
            if(company is null)
                throw new CompanyNotFoundException(id);
            return company;
        }

        public ApiBaseResponse GetAllCompanies(bool trackChanges)
        {
            var companies = _repository.Company.GetAllCompaniesAsync(trackChanges);
            var companiesDto = _mapper.Map<IEnumerable<CompanyDto>>(companies);
            return new ApiOkResponse<IEnumerable<CompanyDto>>(companiesDto);
        }  


        public ApiBaseResponse GetCompany(Guid companyId, bool trackChanges)
        {
            var company = _repository.Company.GetCompanyAsync(companyId, trackChanges);
            if (company is null)
                return new CompanyNotFoundResponse(companyId);
            var companyDto = _mapper.Map<CompanyDto>(company);
            return new ApiOkResponse<CompanyDto>(companyDto);
        }
    }
}
