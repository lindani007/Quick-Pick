using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace QuickPick_Customer.QuieckPickCustomer.Models
{
    public class Aisle
    {
        public int AsileId { get; set; }
        public string? AisleName { get; set; }
        public string? AisleDescription { get; set; }
        public ImageSource? ImageSourceUrl { get; set; }
        public byte[]? AisleImageUrl { get; set; }
    }
}
