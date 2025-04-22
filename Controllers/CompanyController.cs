using DapperDemo.Contracts;
using DapperDemo.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DapperDemo.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class CompanyController : ControllerBase
	{
		private readonly ICompanyRepo _companyRepo;

		public CompanyController(ICompanyRepo companyRepo) => _companyRepo = companyRepo;


		[HttpGet]
		public async Task<IActionResult> GetCompanies()
		{
			try
			{
				var companies = await _companyRepo.GetCompanies();

				if(companies is null) return NotFound();
				return Ok(companies);
			}
			catch(Exception ex)
			{
				return StatusCode(500, ex.Message);
			}
		}


		[HttpGet("{id}", Name = "CompanyById")]
		public async Task<IActionResult> GetCompanyById(int id)
		{
			try
			{
				var company = await _companyRepo.GetCompanyById(id);

				if (company is null) return NotFound();
				return Ok(company);
			}
			catch (Exception ex)
			{
				return StatusCode(500, ex.Message);
			}
		}


		[HttpPost]
		public async Task<IActionResult> AddCompany([FromBody]AddCompanyDTO company)
		{
			try
			{
				var newCompany = await _companyRepo.CreateCompany(company);

				if (newCompany is null) return NotFound();

				return CreatedAtRoute("CompanyById", new { id = newCompany.Id }, newCompany);
			}
			catch (Exception ex)
			{
				return StatusCode(500, ex.Message);
			}
		}



		[HttpPut("{id}")]
		public async Task<IActionResult> UpdateCompany(int id, [FromBody] UpdateCompanyDTO company)
		{
			try
			{
				var dbCompnay = _companyRepo.GetCompanyById(id);
				if (dbCompnay is null) return NotFound();

				await _companyRepo.UpdateCompany(id, company);
				return Ok("Companay updated successfully");
			}
			catch (Exception ex)
			{
				return StatusCode(500, ex.Message);
			}
		}


		[HttpDelete("{id}")]
		public async Task<IActionResult> DeleteCompany(int id)
		{
			try
			{
				var dbCompnay = _companyRepo.GetCompanyById(id);
				if (dbCompnay is null) return NotFound();

				await _companyRepo.DeleteCompany(id);
				return Ok("Companay deleted successfully");
			}
			catch (Exception ex)
			{
				return StatusCode(500, ex.Message);
			}
		}

		[HttpGet("ByEmployeeId/{id}")]
		public async Task<IActionResult> GetCompanyForEmployee(int id)
		{
			try
			{
				var company = await _companyRepo.GetCompanyByEmployeeId(id);
				if (company is null) return NotFound();

				return Ok(company);
			}
			catch (Exception ex)
			{
				return StatusCode(500, ex.Message);
			}
		}


		[HttpGet("{id}/MultipleResult")]
		public async Task<IActionResult> GetMultipleReslt(int id)
		{
			try
			{
				var company = await _companyRepo.GetMultipleResult(id);
				if (company is null) NotFound();

				return Ok(company);
			}
			catch (Exception ex)
			{
				return StatusCode(500, ex.Message);
			}
		}


		[HttpGet("MultipleMapping")]
		public async Task<IActionResult> GetMultipleMapping()
		{
			try
			{
				var company = await _companyRepo.MultipleMapping();

				return Ok(company);
			}
			catch (Exception ex)
			{
				return StatusCode(500, ex.Message);
			}
		}


		[HttpPost("CreateMultipleCompanies")]
		public async Task<IActionResult> CreateMultipleCompanies([FromBody] List<AddCompanyDTO> companies)
		{
			try
			{
				await _companyRepo.CreateMultipleCompanies(companies);
				return Ok("Companies Added Successfully");
			}
			catch (Exception ex)
			{
				return StatusCode(500, ex.Message);
			}
		}
	}
}
