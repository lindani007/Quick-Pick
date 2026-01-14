using System;
using System.Collections.Generic;
using System.Text;

namespace QuickPick_Employye_Packing.QuickPick_Empleyee_Packing_Folder.Models
{
    public  class OrderedItems
    {
        public int TransactionId { get; set; }
        public int OrderId { get; set; }
        public int OrderedId { get; set; }
        public string? Code { get; set; }
        public DateTime TransactionDtae { get; set; }
        public int ItemId { get; set; }
        public string? ItemName { get; set; }
        public int Quantity { get; set; }
        public double Price { get; set; }
        public ImageSource? ImageSourceUrl { get; set; }
        public double TotalAmount { get; set; }
        public string? Packed_By { get; set; }
    }
}
