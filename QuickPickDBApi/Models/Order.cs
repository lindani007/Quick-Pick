using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuickPickDBApi.Models
{
    public class Order
    {
        [Key]
        [Column("Order_id")]
        public int Id { get; set; }
        public string? Status { get; set; }
        public string? Order_Number { get; set; }
        public int Quantity { get; set; }
        public string? Code { get; set; }
        public double Price { get; set; }
        public DateTime Occured_On { get; set; }
    }
}
