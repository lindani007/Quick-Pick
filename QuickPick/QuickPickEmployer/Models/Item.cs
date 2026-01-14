using System;
using System.Collections.Generic;
using System.Text;

namespace QuickPick.QuickPickEmployer.Models
{
    public class Item
    {
        public int ItemId { get; set; }
        public string? ItemName { get; set; }
        public double ItemPrice { get; set; }
        public string? ItemDescription { get; set; }
        public int ItemQuantity {  get; set; }
        public ImageSource? ImageSource { get; set; }
        public byte[]? ItemImageUrl { get; set; }
        public int AisleId { get; set; }
        public string? AisleName { get; set; }
    }
}
