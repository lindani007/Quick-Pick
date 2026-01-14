using System;
using System.Collections.Generic;
using System.Text;

namespace QuickPickDBApiService.Models.ApiModels
{
    public class Aisle
    {
        public int Id { get; set; }
        public string? Aisle_Name { get; set; }
        public string? Description { get; set; }
        public string? ImageUrl { get; set; }
    }
}
