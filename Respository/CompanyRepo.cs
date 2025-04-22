using Dapper;
using DapperDemo.Context;
using DapperDemo.Contracts;
using DapperDemo.DTO;
using DapperDemo.Entities;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.IdentityModel.Tokens;
using System.Data;

namespace DapperDemo.Respository
{
	public class CompanyRepo : ICompanyRepo
	{
		private readonly DapperContext _context;

		public CompanyRepo(DapperContext context) => _context = context;

		public async Task<Company> CreateCompany(AddCompanyDTO company)
		{
			try
			{
				var query = "INSERT INTO Companies (Name, Address, Country) VALUES (@Name, @Address, @Country)" +
					"SELECT CAST(SCOPE_IDENTITY() AS int)";

				var parameters = new DynamicParameters();
				parameters.Add("Name", company.Name, DbType.String);
				parameters.Add("Address", company.Address, DbType.String);
				parameters.Add("Country", company.Country, DbType.String);

				using(var con = _context.CreateConnection())
				{
					var id = await con.QuerySingleAsync<int>(query, parameters);

					var newCompnay = new Company
					{
						Id = id,
						Name = company.Name,
						Address = company.Address,
						Country = company.Country,
					};

					return newCompnay;
				}
			}
			catch (Exception ex)
			{
				throw new Exception(ex.InnerException?.Message ?? ex.Message);
			}
		}

		public async Task DeleteCompany(int id)
		{
			try
			{
				var query = "DELETE FROM Companies WHERE Id = @Id";

				using(var con = _context.CreateConnection())
				{
					await con.ExecuteAsync(query, new { id });
				}
			}
			catch (Exception ex)
			{
				throw new Exception(ex.InnerException?.Message ?? ex.Message);
			}
		}

		public async Task<List<Company>> GetCompanies()
		{
			try
			{
				var query = "SELECT * FROM Companies";

				using(var con = _context.CreateConnection())
				{
					var companies = await con.QueryAsync<Company>(query);
					return companies.ToList();
				}
			}
			catch (Exception ex)
			{
				throw new Exception(ex.InnerException?.Message ?? ex.Message);
			}
		}

		public async Task<Company> GetCompanyByEmployeeId(int id)
		{
			try
			{
				var procedureName = "ShowCompanyByEmployeeId";
				var parameters = new DynamicParameters();
				parameters.Add("Id", id, DbType.Int32, ParameterDirection.Input);

				using(var con = _context.CreateConnection())
				{
					var company = await con.QueryFirstOrDefaultAsync<Company>
						(procedureName, parameters, commandType: CommandType.StoredProcedure);

					return company;
				}

			}
			catch (Exception ex)
			{
				throw new Exception(ex.InnerException?.Message ?? ex.Message);
			}
		}

		public async Task<Company> GetCompanyById(int id)
		{
			try
			{
				var query = "SELECT * FROM Companies WHERE Id = @Id";

				using (var con = _context.CreateConnection())
				{
					var company = await con.QuerySingleOrDefaultAsync<Company>(query, new { id });
					return company;
				}
			}
			catch (Exception ex)
			{
				throw new Exception(ex.InnerException?.Message ?? ex.Message);
			}
		}

		public async Task<Company> GetMultipleResult(int id)
		{
			try
			{
				var query = "SELECT * FROM Companies WHERE Id = @Id;" +
					"SELECT * FROM Employees WHERE CompanyId = @Id";

				using(var con = _context.CreateConnection())
				using(var multi = await con.QueryMultipleAsync(query, new { id }))
				{
					var company = await multi.ReadSingleOrDefaultAsync<Company>();
					if(company is not null)
						company.Employees = (await multi.ReadAsync<Employee>()).ToList();

					return company;
				}
			}
			catch (Exception ex)
			{
				throw new Exception(ex.InnerException?.Message ?? ex.Message);
			}
		}

		public async Task<List<Company>> MultipleMapping()
		{
			try 
			{
				var query = "SELECT * FROM Companies c JOIN Employees e ON c.Id = e.CompanyId";

				using(var con = _context.CreateConnection())
				{
					var companyDict = new Dictionary<int, Company>();

					var companies = await con.QueryAsync<Company, Employee, Company>(
						query, (company, employee) =>
						{
							if (!companyDict.TryGetValue(company.Id, out var currentCompany))
							{
								currentCompany = company;
								companyDict.Add(currentCompany.Id, currentCompany);
							};

							currentCompany.Employees.Add(employee);

							return currentCompany;
						}
					);

					return companies.Distinct().ToList();
				}


			}
			catch (Exception ex)
			{
				throw new Exception(ex.InnerException?.Message ?? ex.Message);
			}
		}

		public async Task UpdateCompany(int id, UpdateCompanyDTO company)
		{
			try
			{
				var query = "UPDATE Companies SET Name = @Name, Address = @Address, Country = @Country WHERE Id = @Id";

				var parmeters = new DynamicParameters();
				parmeters.Add("Id", id, DbType.Int32);
				parmeters.Add("Name", company.Name, DbType.String);
				parmeters.Add("Address", company.Address, DbType.String);
				parmeters.Add("Country", company.Country, DbType.String);

				using(var con = _context.CreateConnection())
				{
					await con.ExecuteAsync(query, parmeters);
				}


			}
			catch (Exception ex)
			{
				throw new Exception(ex.InnerException?.Message ?? ex.Message);
			}
		}


		public async Task CreateMultipleCompanies(List<AddCompanyDTO> companies)
		{
			try
			{
				var query = "INSERT INTO Companies (Name, Address, Country) VALUES (@Name, @Address, @Country)";

				using (var con = _context.CreateConnection())
				{
					con.Open();

					using( var trans = con.BeginTransaction())
					{
						try
						{
							foreach (var company in companies)
							{
								var parameters = new DynamicParameters();

								parameters.Add("Name", company.Name, DbType.String);
								parameters.Add("Address", company.Address, DbType.String);
								parameters.Add("Country", company.Country, DbType.String);

								await con.ExecuteAsync(query, parameters, transaction: trans);
							}
							trans.Commit();
						}
						catch (Exception ex)
						{
							trans.Rollback();
							throw new Exception(ex.InnerException?.Message ?? ex.Message);
						}
					}
				}
			}
			catch(Exception ex)
			{
				throw new Exception(ex.InnerException?.Message ?? ex.Message);
			}
		}
	}
}
