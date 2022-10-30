using Microsoft.AspNetCore.Mvc;
using Presentation.ActionFilters; 
using Presentation.ModelBinders;
using Service.Contract;
using Shared.DataTransferObjects;
using Shared.RequestFeatures;
using System.Text.Json;

namespace CompanyEmployees.Presentation.Controllers
{
    [Route("api/companies")]
    [ApiController]
    public class CompaniesController : ControllerBase
    {
        private readonly IServiceManager _serviceManager;

        public CompaniesController(IServiceManager serviceManager)
        {
            _serviceManager = serviceManager;
        }

        [HttpGet]
        public async Task<ActionResult> GetCompanies([FromQuery] CompanyParameters companyParameters)
        {
            var companiesPagedResult = await _serviceManager.CompanyService.GetAllCompaniesAsync(companyParameters, trackChanges: false);
            Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(companiesPagedResult.metaData));
            return Ok(companiesPagedResult.companies);
        }

        [HttpGet("{id:guid}", Name = "CompanyById")]
        public async Task<IActionResult> GetCompany(Guid id)
        {
            var company = await _serviceManager.CompanyService.GetCompanyAsync(id, trackChanges: false);
            return Ok(company);
        }

        [HttpGet("collection/{companyIds}", Name = "CompanyCollection")]
        public async Task<IActionResult> GetCompanyCollection([ModelBinder(BinderType = typeof(ArrayModelBinder))]IEnumerable<Guid> companyIds)
        {
            var companies = await _serviceManager.CompanyService.GetByIdsAsync(companyIds, trackChanges: false);
            return Ok(companies);
        }

        //Company validation is one using action filter instead of if else conditions.
        [HttpPost]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> CreateCompany([FromBody] CompanyForCreationDto company)
        {
            var createdCompany = await _serviceManager.CompanyService.CreateCompanyAsync(company);
            return CreatedAtRoute("CompanyById", new { id = createdCompany.Id }, createdCompany);
        }

        [HttpPost("collection")]
        public async Task<IActionResult> CreateCompanyCollection([FromBody] IEnumerable<CompanyForCreationDto> companyCollection)
        {
            var result = await _serviceManager.CompanyService.CreateCompanyCollectionAsync(companyCollection);
            return CreatedAtRoute("CompanyCollection", new {companyIds = result.ids}, result.companies);
        }

        [HttpDelete("{companyId:Guid}")]
        public async Task<IActionResult> DeleteCompany(Guid companyId)
        {
            await _serviceManager.CompanyService.DeleteCompanyAsync(companyId, trackChanges: false);
            return NoContent();
        }

        //Company validation is one using action filter instead of if else conditions.
        [HttpPut("{companyId:Guid}")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]  
        public async Task<IActionResult> UpdateCompany(Guid companyId, [FromBody] CompanyForUpdateDto companyForUpdate)
        {
                    await _serviceManager.CompanyService.UpdateCompanyAsync(companyId, companyForUpdate, trackChanges: true);
                    return NoContent();
        }
    }
}
