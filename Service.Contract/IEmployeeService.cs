using Entities.LinkModels;
using Entities.Models;
using Shared.DataTransferObjects;
using Shared.RequestFeatures;
using System.Dynamic;

namespace Service.Contract
{
    public interface IEmployeeService
    {
        Task<(LinkResponse linkResponse, MetaData metadata)> GetEmployeesAsync(Guid companyId, LinkParameters linkParameters, 
            bool trackChanges);
        Task<EmployeeDto> GetEmployeeAsync(Guid companyId, Guid id, bool trackChanges);
        Task<EmployeeDto> CreateEmployeeForCompanyAsync(Guid companyId, EmployeeForCreationDto employeeForCreation, bool trackChanges);
        Task DeleteEmployeeForCompany(Guid companyId, Guid employeeId, bool trackChanges);
        Task UpdateEmployeeForCompany(Guid companyId, Guid employeeId, EmployeeForUpdateDto employeeForUpdate, bool companyTrackChanges, bool employeeTrackChanges);
        Task<(EmployeeForUpdateDto employeeToPatch, Employee employeeEntity)> GetEmployeeForPatchAsync(Guid companyId, Guid employeeId, 
            bool companyTrackChanges, bool employeeTrackChanges);
        Task SaveChangesForPatchAsync(EmployeeForUpdateDto employeeForPatch, Employee employeeEntity);
    }
}
