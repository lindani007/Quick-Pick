using Microsoft.JSInterop.Infrastructure;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuickPickDBApi.Models
{
    public class BoughtItem
    {
        [Key]
        public int Id { get; set; }
        public int TransactionId { get; set; }
        public int Order_Id { get; set; }
        public DateTime TransactionDtae { get; set; }
        public int ItemId { get; set; }
        public string? ItemName { get; set; }
        public int Quantity { get; set; }
        public double Price { get; set; }
        public string? ImageSourceUrl { get; set; }
        public double TotalAmount { get; set; }
        public string? Packed_By { get; set; }
    }
}
