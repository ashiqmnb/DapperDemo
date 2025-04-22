using DapperDemo.DTO;
using DapperDemo.Entities;

namespace DapperDemo.Contracts
{
	public interface ICompanyRepo
	{
		Task<List<Company>> GetCompanies();
		Task<Company> GetCompanyById(int id);
		Task<Company> CreateCompany(AddCompanyDTO company);
		Task UpdateCompany(int id, UpdateCompanyDTO company);
		Task DeleteCompany(int id);
		Task<Company> GetCompanyByEmployeeId(int id);
		Task<Company> GetMultipleResult(int id);
		Task<List<Company>> MultipleMapping();
		Task CreateMultipleCompanies(List<AddCompanyDTO> companies);
	}

}
