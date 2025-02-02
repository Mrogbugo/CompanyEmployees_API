﻿using Share.DataTransferObjects;

namespace Service.Contracts
{
    public interface ICompanyService
    {
       IEnumerable<CompanyDto> GetAllCompanies(bool trackChanges);
        CompanyDto GetCompany(Guid companyId, bool trackChanges);
    }
}
