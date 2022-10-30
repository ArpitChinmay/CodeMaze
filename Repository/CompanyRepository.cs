using Contracts;
using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Repository.Extensions;
using Shared.RequestFeatures;

namespace Repository
{
    public class CompanyRepository : RepositoryBase<Company>, ICompanyRepository
    {
        public CompanyRepository(RepositoryContext repositoryContext) : base(repositoryContext)
        {

        }

        public async Task<PagedList<Company>> GetAllCompaniesAsync(CompanyParameters companyParameters, bool trackChanges)
        {
            List<Company>? companies;
            if (companyParameters.Country != null)
            {
                    companies = await FindByCondition(c => c.Country.Equals(companyParameters.Country), trackChanges)
                    .Search(companyParameters.SearchTerm)
                    .OrderBy(c => c.Name)
                    //.Skip((companyParameters.PageNumber - 1) * companyParameters.PageSize)
                    //.Take(companyParameters.PageSize)
                    .ToListAsync();
            }
            else
            {
                companies = await FindAll(trackChanges).OrderBy(c => c.Name).ToListAsync();
            }

            return PagedList<Company>.ToPagedList(companies, companyParameters.PageNumber, companyParameters.PageSize);
        }

        public async Task<Company> GetCompanyAsync(Guid companyId, bool trackChanges) =>  
            await FindByCondition(c => c.Id.Equals(companyId), trackChanges).SingleOrDefaultAsync();

        public void CreateCompany(Company company) => Create(company);
        
        public async Task<IEnumerable<Company>> GetByIdsAsync(IEnumerable<Guid> CompanyIds, bool trackChanges)
        {
            return await FindByCondition(x => CompanyIds.Contains(x.Id), trackChanges).ToListAsync();
        }

        public void DeleteCompany(Company company)
        {
            Delete(company);
        }
    }
}
