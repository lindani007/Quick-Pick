using System.ComponentModel.DataAnnotations;

namespace QuickPickDBApi.Models
{
    public class Sale
    {
        [Key]
        public int Transaction_Id { get; set; }
        public string? Packed_By { get; set; }
        public int Quantity { get; set; }
        public double TotalAmount { get; set; }
        public DateTime TransactionDate { get; set; }
    }
}
