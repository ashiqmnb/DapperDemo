﻿namespace DapperDemo.Entities
{
	public class Employee
	{
        public int Id { get; set; }
		public string? Name { get; set; }
		public int Age { get; set; }
		public string? Position { get; set; }
		public decimal Salary { get; set; }
		public int CompanyId { get; set; }
	}
}
