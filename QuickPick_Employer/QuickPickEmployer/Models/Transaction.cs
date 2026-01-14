using System;
using System.Collections.Generic;
using System.Text;

namespace QuickPick_Employer.QuickPickEmployer.Models
{
    public class Transaction
    {
        public int TransactionId { get; set; }
        public string? Packed_By { get; set; }
        public int Quantity { get; set; }
        public double TotalAmount { get; set; }
        public DateTime TransactionDate { get; set; }
    }
}
