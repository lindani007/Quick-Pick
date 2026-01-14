using System;
using System.Collections.Generic;
using System.Text;

namespace QuickPickDBApiService.Models.ApiModels
{
    public class Order
    {
        public int Id { get; set; }
        public string? Order_Number { get; set; }
        public int Quantity { get; set; }
        public string? Status { get; set; }
        public string? Code { get; set; }
        public double Price { get; set; }
        public DateTime Occured_On { get; set; }
    }
}
