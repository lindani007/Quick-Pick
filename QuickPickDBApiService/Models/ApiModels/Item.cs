using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace QuickPickDBApiService.Models.ApiModels
{
    public class Item
    {
        public int ID { get; set; }
        public string? Item_Name { get; set; }
        public string? Description { get; set; }
        public int Quantity { get; set; }
        public int LeftQuantity { get; set; }
        public double Price { get; set; }
        public string? ImageUrl { get; set; }
        public int Aisle_Id { get; set; }
    }
}
