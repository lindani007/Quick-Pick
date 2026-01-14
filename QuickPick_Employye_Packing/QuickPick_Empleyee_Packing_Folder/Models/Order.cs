using System;
using System.Collections.Generic;
using System.Text;

namespace QuickPick_Employye_Packing.QuickPick_Empleyee_Packing_Folder.Models
{
    public class Order
    {
        public int OrderId { get; set; }
        public string? OrderNumber { get; set; }
        public string? Status { get; set; }
        public string? Code { get; set; }
        public int OrderedItemsQty { get; set; }
        public double TotalAmount { get; set; }
        public DateTime OrderDate { get; set; }
        public string? OrderedBy { get; set; }
    }
}
