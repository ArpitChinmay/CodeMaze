using Entities.LinkModels;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Presentation.ActionFilters;
using Service.Contract;
using Shared.DataTransferObjects;
using Shared.RequestFeatures;
using System.Text.Json;

namespace Presentation.Controllers
{
    [Route("api/companies/{companyId}/employees")]
    public class EmployeesController : ControllerBase
    {
        private readonly IServiceManager _service;
        public EmployeesController(IServiceManager service)
        {
            _service = service;
        }

        [HttpGet]
        [ServiceFilter(typeof(ValidateMediaTypeAttribute))]
        public async Task<IActionResult> GetEmployeesForCompany(Guid companyId, [FromQuery] EmployeeParameters employeeParameters)
        {
            var linkParams = new LinkParameters(employeeParameters, HttpContext);
            var pagedResult = await _service.EmployeeService.GetEmployeesAsync(companyId, linkParams, trackChanges: false);
            Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(pagedResult.metadata));
            return pagedResult.linkResponse.HasLinks ? Ok(pagedResult.linkResponse.LinkedEntities) : Ok(pagedResult.linkResponse.ShapedEntities);
        }

        [HttpGet("{employeeId:Guid}", Name = "GetEmployeesForCompany")]
        public async Task<IActionResult> GetEmployee(Guid companyId, Guid employeeId)
        {
            var employee = await _service.EmployeeService.GetEmployeeAsync(companyId, employeeId, trackChanges: false);
            return Ok(employee);
        }

        // Doing employee validation using ActionFilter attributes instead of regular if-else methods.
        [HttpPost]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> CreateEmployeeForCompany(Guid companyId, [FromBody] EmployeeForCreationDto employee)
        {
            var employeeToReturn = await _service.EmployeeService.CreateEmployeeForCompanyAsync(companyId, employee, 
                trackChanges: false);
            return CreatedAtRoute("GetEmployeesForCompany", new { companyId, employeeId = employeeToReturn.Id }, 
                employeeToReturn);
        }

        [HttpDelete("{employeeId:Guid}")]
        public async Task<IActionResult> DeleteEmployee(Guid companyId, Guid employeeId)
        {
            await _service.EmployeeService.DeleteEmployeeForCompany(companyId, employeeId, trackChanges: false);
            return NoContent();
        }

        // Doing employee validation using ActionFilter attribute instead of regular if-else methods.
        [HttpPut("{employeeId:Guid}")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> UpdateEmployeeForCompany(Guid companyId, Guid employeeId, 
            [FromBody] EmployeeForUpdateDto employeeForUpdate)
        {
            await _service.EmployeeService.UpdateEmployeeForCompany(companyId, employeeId, employeeForUpdate, companyTrackChanges: false, employeeTrackChanges: true);
            return NoContent();
        }

        [HttpPatch("{employeeId:Guid}")]
        public async Task<IActionResult> PartiallyUpdateEmployeeForCompany(Guid companyId, Guid employeeId, 
            [FromBody] JsonPatchDocument<EmployeeForUpdateDto> patchDoc)
        {
            if(patchDoc is null)
            {
                return BadRequest("patchDoc object sent from client is null");
            }
            else
            {
                var result = await _service.EmployeeService.GetEmployeeForPatchAsync(companyId, employeeId, companyTrackChanges: false, employeeTrackChanges: true);
                patchDoc.ApplyTo(result.employeeToPatch);
                TryValidateModel(result.employeeToPatch);
                if (ModelState.IsValid)
                {
                    await _service.EmployeeService.SaveChangesForPatchAsync(result.employeeToPatch, result.employeeEntity);
                    return NoContent();
                }
                else
                {
                    return UnprocessableEntity(ModelState);
                }
            }
        }
    }
}
