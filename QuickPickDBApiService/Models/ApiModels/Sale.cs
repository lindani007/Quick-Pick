using System;
using System.Collections.Generic;
using System.Text;

namespace QuickPickDBApiService.Models.ApiModels
{
    public class Sale
    {
        public int Transaction_Id { get; set; }
        public string? Packed_By { get; set; }
        public int Quantity { get; set; }
        public double TotalAmount { get; set; }
        public DateTime TransactionDate { get; set; }
    }
}
