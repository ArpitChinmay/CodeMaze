using AutoMapper;
using Contracts;
using Entities.Exceptions;
using Entities.LinkModels;
using Entities.Models;
using Service.Contract;
using Shared.DataTransferObjects;
using Shared.RequestFeatures;
using System.Dynamic;

namespace Service
{
    internal sealed class EmployeeService : IEmployeeService
    {
        private readonly IRepositoryManager _repository;
        private readonly ILoggerManager _loggerManager;
        private readonly IMapper _mapper;
        private readonly IEmployeeLinks _employeeLinks;

        public EmployeeService(IRepositoryManager repositoryManager, ILoggerManager loggerManager, IMapper mapper, 
            IEmployeeLinks employeeLinks)
        {
            _repository = repositoryManager;
            _loggerManager = loggerManager;
            _mapper = mapper;
            _employeeLinks = employeeLinks;
        }

        public async Task<(LinkResponse linkResponse, MetaData metadata)> GetEmployeesAsync(Guid companyId, 
            LinkParameters linkParameters, bool trackChanges)
        {
            if(!linkParameters.EmployeeParameters.ValidAgeRange)
            {
                throw new MaxAgeRangeBadRequestException();
            }
            
            await CheckIfCompanyExists(companyId, trackChanges);
            
            var employeesWithMetaData = await _repository.Employee.GetEmployeesAsync(companyId, linkParameters.EmployeeParameters, trackChanges);
            var employeeDto = _mapper.Map<IEnumerable<EmployeeDto>>(employeesWithMetaData);
            var links = _employeeLinks.TryGenerateLinks(employeeDto, linkParameters.EmployeeParameters.Fields, companyId, linkParameters.Context);
            return (linkResponse: links, metadata: employeesWithMetaData.MetaData);
        }

        public async Task<EmployeeDto> GetEmployeeAsync(Guid companyId, Guid id, bool trackChanges)
        {
            await CheckIfCompanyExists(companyId, trackChanges);
            
            var employeeFromDb = await _repository.Employee.GetEmployeeAsync(companyId, id, trackChanges);

            var employeeDto = _mapper.Map<EmployeeDto>(employeeFromDb);
            return employeeDto;
        }

        public async Task<EmployeeDto> CreateEmployeeForCompanyAsync(Guid companyId, EmployeeForCreationDto employeeForCreationDto, 
            bool trackChanges)
        {
            await CheckIfCompanyExists(companyId, trackChanges);
            
            var employeeEntity = _mapper.Map<Employee>(employeeForCreationDto);
            _repository.Employee.CreateEmployeeForCompany(companyId, employeeEntity);
            await _repository.SaveAsync();

            var employeeToReturn = _mapper.Map<EmployeeDto>(employeeEntity);
            return employeeToReturn;
        }

        public async Task DeleteEmployeeForCompany(Guid companyId, Guid employeeId, bool trackChanges)
        {
            await CheckIfCompanyExists(companyId, trackChanges);
            
            var employeeInCompany = await GetEmployeeForCompanyAndCheckIfItExists(companyId, employeeId, trackChanges);
            
            _repository.Employee.DeleteEmployee(employeeInCompany);
            await _repository.SaveAsync();
        }

        public async Task UpdateEmployeeForCompany(Guid companyId, Guid employeeId, EmployeeForUpdateDto employeeForUpdate, 
            bool companyTrackChanges, bool employeeTrackChanges)
        {
            await CheckIfCompanyExists(companyId, companyTrackChanges);

            var employeeToUpdate = await GetEmployeeForCompanyAndCheckIfItExists(companyId, employeeId, employeeTrackChanges);
            
            _mapper.Map(employeeForUpdate, employeeToUpdate);
            await _repository.SaveAsync();
        }

        public async Task<(EmployeeForUpdateDto employeeToPatch, Employee employeeEntity)> GetEmployeeForPatchAsync(Guid companyId, Guid employeeId,
            bool companyTrackChanges, bool employeeTrackChanges)
        {
            await CheckIfCompanyExists(companyId, companyTrackChanges);
            
            var employeeEntity = await GetEmployeeForCompanyAndCheckIfItExists(companyId, employeeId, employeeTrackChanges);
                
            var employeeToPatch = _mapper.Map<EmployeeForUpdateDto>(employeeEntity);
            return (employeeToPatch: employeeToPatch, employeeEntity:  employeeEntity) ;
        }


        public async Task SaveChangesForPatchAsync(EmployeeForUpdateDto employeeToPatch, Employee employeeEntity)
        {
            _mapper.Map(employeeToPatch, employeeEntity);
            await _repository.SaveAsync();
        }

        private async Task CheckIfCompanyExists(Guid companyId, bool trackChanges)
        {
            var company = await _repository.Company.GetCompanyAsync(companyId, trackChanges);
            if(company is null)
            {
                throw new CompanyNotFoundException(companyId);
            }
        }

        private async Task<Employee> GetEmployeeForCompanyAndCheckIfItExists(Guid companyId, Guid employeeId, bool trackChanges)
        {
            var employeeDb = await _repository.Employee.GetEmployeeAsync(companyId, employeeId, trackChanges);
            if(employeeDb is null)
            { throw new EmployeeNotFoundException(employeeId); }

            return employeeDb;
        }
    }
}
