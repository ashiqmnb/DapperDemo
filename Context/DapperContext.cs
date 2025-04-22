using Microsoft.Data.SqlClient;
using System.Data;

namespace DapperDemo.Context
{
	public class DapperContext
	{
		private readonly IConfiguration _configuration;
		private readonly string _conStr;

		public DapperContext(IConfiguration configuration)
		{
			_configuration = configuration;
			_conStr = _configuration.GetConnectionString("DefaultConnection");
		}

		public IDbConnection CreateConnection() => new SqlConnection(_conStr);
	}
}
