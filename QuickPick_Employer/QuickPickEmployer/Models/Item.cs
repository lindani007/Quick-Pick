using System;
using System.Collections.Generic;
using System.Text;

namespace QuickPick_Employer.QuickPickEmployer.Models
{
    public class Item
    {
        public int ItemId { get; set; }
        public string? ItemName { get; set; }
        public double ItemPrice { get; set; }
        public string? ItemDescription { get; set; }
        public int ItemQuantity {  get; set; }
        public int LeftQuantity { get; set; }
        public ImageSource? ImageSource { get; set; }
        public byte[]? ItemImageUrl { get; set; }
        public string? ImageUrl { get; set; }
        public int AisleId { get; set; }
    }
}
