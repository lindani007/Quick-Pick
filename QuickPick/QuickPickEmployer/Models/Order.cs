using System;
using System.Collections.Generic;
using System.Text;

namespace QuickPick.QuickPickEmployer.Models
{
    public class Order
    {
        public int OrderId { get; set; }
        public int OrderedItemsQty { get; set; }
        public double TotalAmount { get; set; }
        public DateTime OrderDate { get; set; }
        public int ItemId { get; set; }
    }
}
