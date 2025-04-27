using Contracts;
using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Respository.Extensions;
using Share.RequestFeatures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Respository
{
    public class EmployeeRepository : RepositoryBase<Employee>, IEmployeeRepository
    { 
        public EmployeeRepository(RepositoryContext repositoryContext): base(repositoryContext) 
        { 

        }

        public void CreateEmployeeForCompany(Guid companyId, Employee employee)
        {
            employee.CompanyId = companyId;
            Create(employee);
        }

        // in paging the below method works well with small amount of data --- first method 

        //public async Task<PagedList<Employee>> GetEmployeesAsync(Guid companyId, EmployeeParameters employeeParameters, bool trackChanges)
        //{
        //    var employees = await FindByCondition(e => e.CompanyId.Equals(companyId) && (e.Age >= employeeParameters.MinAge && e.Age <= employeeParameters.MaxAge), trackChanges)
                   // .FilterEmployees(employeeParameters.MinAge, employeeParameters.MaxAge)
                   //.Search(employeeParameters.SearchTerm)
                   //.Sort(employeeParameter.OrderBy)
        //        .OrderBy(e => e.Name)
        //        .Skip((employeeParameters.PageNumber -1) * employeeParameters.PageSize)
        //        .ToListAsync();

        //    return PagedList<Employee>
        //        .ToPagedList(employees, employeeParameters.PageNumber, employeeParameters.PageSize);
        //}  


        // Below method works well with bigger tables with millons of rows --- second method 

        public async Task<PagedList<Employee>> GetEmployeesAsync(Guid companyId, EmployeeParameters employeeParameters, bool trackChanges)
        {
            var employees = await FindByCondition(e => e.CompanyId.Equals(companyId) && (e.Age >= employeeParameters.MinAge && e.Age <= employeeParameters.MaxAge), trackChanges)
                .FilterEmployees(employeeParameters.MinAge, employeeParameters.MaxAge)
                .Search(employeeParameters.SearchTerm)
                .Sort(employeeParameters.OrderBy)
                .OrderBy(e => e.Name)
                .Skip((employeeParameters.PageNumber - 1) * employeeParameters.PageSize)
                .Take(employeeParameters.PageSize)
                .ToListAsync();

            var count = await FindByCondition(e => e.CompanyId.Equals(companyId), trackChanges).CountAsync();

            return PagedList<Employee>
                .ToPagedList(employees, employeeParameters.PageNumber, employeeParameters.PageSize);
        }

        public async Task <Employee> GetEmployeeAsync(Guid companyId, Guid id, bool trackChanges) =>
           await FindByCondition(e => e.CompanyId.Equals(companyId) && e.Id.Equals(id), trackChanges)
            .SingleOrDefaultAsync();  

        public void DeleteEmployee(Employee employee) => Delete(employee);


    }
}
