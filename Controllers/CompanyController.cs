namespace DapperDemo.Controllers
{
	using DapperDemo.Contracts;
	using DapperDemo.DTO;
	using Microsoft.AspNetCore.Http;
	using Microsoft.AspNetCore.Mvc;

	/// <summary>
	/// Defines the <see cref="CompanyController" />
	/// </summary>
	[Route("api/[controller]")]
	[ApiController]
	public class CompanyController : ControllerBase
	{
		/// <summary>
		/// Defines the _companyRepo
		/// </summary>
		private readonly ICompanyRepo _companyRepo;

		/// <summary>
		/// Initializes a new instance of the <see cref="CompanyController"/> class.
		/// </summary>
		/// <param name="companyRepo">The companyRepo<see cref="ICompanyRepo"/></param>
		public CompanyController(ICompanyRepo companyRepo) => _companyRepo = companyRepo;

		/// <summary>
		/// The GetCompanies
		/// </summary>
		/// <returns>The <see cref="Task{IActionResult}"/></returns>
		[HttpGet]
		public async Task<IActionResult> GetCompanies()
		{
			try
			{
				var companies = await _companyRepo.GetCompanies();

				if (companies is null) return NotFound();
				return Ok(companies);
			}
			catch (Exception ex)
			{
				return StatusCode(500, ex.Message);
			}
		}

		/// <summary>
		/// The GetCompanyById
		/// </summary>
		/// <param name="id">The id<see cref="int"/></param>
		/// <returns>The <see cref="Task{IActionResult}"/></returns>
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

		/// <summary>
		/// The AddCompany
		/// </summary>
		/// <param name="company">The company<see cref="AddCompanyDTO"/></param>
		/// <returns>The <see cref="Task{IActionResult}"/></returns>
		[HttpPost]
		public async Task<IActionResult> AddCompany([FromBody] AddCompanyDTO company)
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

		/// <summary>
		/// The UpdateCompany
		/// </summary>
		/// <param name="id">The id<see cref="int"/></param>
		/// <param name="company">The company<see cref="UpdateCompanyDTO"/></param>
		/// <returns>The <see cref="Task{IActionResult}"/></returns>
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

		/// <summary>
		/// The DeleteCompany
		/// </summary>
		/// <param name="id">The id<see cref="int"/></param>
		/// <returns>The <see cref="Task{IActionResult}"/></returns>
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

		/// <summary>
		/// The GetCompanyForEmployee
		/// </summary>
		/// <param name="id">The id<see cref="int"/></param>
		/// <returns>The <see cref="Task{IActionResult}"/></returns>
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

		/// <summary>
		/// The GetMultipleReslt
		/// </summary>
		/// <param name="id">The id<see cref="int"/></param>
		/// <returns>The <see cref="Task{IActionResult}"/></returns>
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

		/// <summary>
		/// The GetMultipleMapping
		/// </summary>
		/// <returns>The <see cref="Task{IActionResult}"/></returns>
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

		/// <summary>
		/// The CreateMultipleCompanies
		/// </summary>
		/// <param name="companies">The companies<see cref="List{AddCompanyDTO}"/></param>
		/// <returns>The <see cref="Task{IActionResult}"/></returns>
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
