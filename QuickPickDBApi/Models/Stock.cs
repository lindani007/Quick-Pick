using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuickPickDBApi.Models
{
    public class Stock
    {
        [Key]
        [Column("Item_Id")]
        public int ID { get; set; }
        public string? Item_Name { get; set; }
        public string? Description { get; set; }
        public int Quantity { get; set; }
        public double Price { get; set; }
        public string? ImageUrl { get; set; }
        public int Aisle_Id { get; set; }
        public DateTime Stock_On { get; set; }
    }
}
