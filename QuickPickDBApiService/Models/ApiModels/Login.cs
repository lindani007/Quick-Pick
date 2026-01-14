using System;
using System.Collections.Generic;
using System.Text;

namespace QuickPickDBApiService.Models.ApiModels
{
    public class Login
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? EmployeeID { get; set; }
        public string? Password { get; set; }
    }
}
