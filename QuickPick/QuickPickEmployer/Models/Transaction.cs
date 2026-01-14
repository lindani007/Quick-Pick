using System;
using System.Collections.Generic;
using System.Text;

namespace QuickPick.QuickPickEmployer.Models
{
    public class Transaction
    {
        public int TransactionId { get; set; }
        public string? UserId { get; set; }
        public int ItemId { get; set; }
        public int Quantity { get; set; }
        public double TotalAmount { get; set; }
        public DateTime TransactionDate { get; set; }
        public int OrderId { get; set; }
    }
}
