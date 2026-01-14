namespace QuickPickSignlaRService.Models
{
    public class Sale
    {
        public int TransactionId { get; set; }
        public string? Packed_By { get; set; }
        public int Quantity { get; set; }
        public double TotalAmount { get; set; }
        public DateTime TransactionDate { get; set; }
    }
}
