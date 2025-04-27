using CompanyEmployees.ActionFilters;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Presentation.ModelBinders;
using Service.Contracts;
using Share.DataTransferObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presentation.Controllers
{
    [Route("api/companies")]
    [ApiController]
    public class CompaniesController : ControllerBase
    {
        private readonly IServiceManager _service;
        public CompaniesController(IServiceManager service) =>

            _service = service;

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task <IActionResult> GetCompanies()
        {
            //   throw new Exception("Exception");
            var companies =
          await  _service.CompanyService.GetAllCompaniesAsync(trackChanges: false);
            return Ok(companies);
        }

        [HttpGet("{id:guid}", Name = "GetCompanyById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> GetCompanyAsync(Guid id)
        {
            var company = await _service.CompanyService.GetCompanyAsync(id, trackChanges: false);
            return Ok(company);
        }

        [HttpPost]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async  Task<IActionResult> CreateComapny([FromBody] CompanyForCreationDto company)
        {
            //if (company is null)
            //    return BadRequest("CompanyForCreation object is null");
            //if (!ModelState.IsValid)
            //    return UnprocessableEntity(ModelState);

            var CreatedCompany = await _service.CompanyService.CreateCompanyAsync(company);
            return CreatedAtRoute("CompanyById", new { id = CreatedCompany.Id }, CreatedCompany);
        }

        [HttpGet("Collection/{ids}", Name = "CompanyCollection")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async  Task<IActionResult> GetCompanyCollection([ModelBinder(binderType: typeof(ArrayModelBinder))] IEnumerable<Guid> ids)
        {

            if (ids is null)
                return BadRequest("Get Company Collection By  Id Is Null");
            var companies = await _service.CompanyService.GetByIdsAsync(ids, trackChanges: false);

            return Ok(companies);
        }

        [HttpPost("collection")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async  Task<IActionResult> CreateCompanyCollection([FromBody] IEnumerable<CompanyForCreationDto> companyCollection)
        {    if (companyCollection is null)
                return BadRequest("Company For Creation DTO Can not be null");
            if (!ModelState.IsValid)
                return UnprocessableEntity(ModelState);
            var res = await _service.CompanyService.CreateCompanyCollectionAsync(companyCollection); 

            return CreatedAtRoute("CompanyCollection", new { res.ids }, res.companies);
        }

        [HttpDelete("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task <IActionResult> DeleteCompany(Guid id)
        {
            _service.CompanyService.DeleteCompanyAsync(id, trackChanges: false);
            return NoContent();

        }

        [HttpPut("{id:guid}")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> UpdateCompany(Guid id, [FromBody] CompanyForUpdateDto company)
        {
            //if (company is null)
            //    return BadRequest("CompanyForUpdateDto object is null"); 
            //if(!ModelState.IsValid)
            //    return UnprocessableEntity();
          await  _service.CompanyService.UpdateCompanyAsync(id, company, trackChanges: true);
            return NoContent();
        }
    }
}