namespace QuickPickSignlaRService.Models
{
    public class BoughtItem
    {
        public int TransactionId { get; set; }
        public int OrderId { get; set; }
        public int OrderedId { get; set; }
        public string? Code { get; set; }
        public DateTime TransactionDtae { get; set; }
        public int ItemId { get; set; }
        public string? ItemName { get; set; }
        public string? Description { get; set; }
        public int Quantity { get; set; }
        public int LeftQuantity { get; set; }
        public double Price { get; set; }
        public string? ImageSourceUrl { get; set; }
        public double TotalAmount { get; set; }
        public string? Packed_By { get; set; }
    }
}
