namespace QuickPickSignlaRService.Models
{
    public class Order
    {
        public int OrderId { get; set; }
        public string? OrderNumber { get; set; }
        public string? Status { get; set; }
        public string? Code { get; set; }
        public int OrderedItemsQty { get; set; }
        public double TotalAmount { get; set; }
        public DateTime OrderDate { get; set; }
        public string? OrderedBy { get; set; }
        public List<BoughtItem>? Items { get; set; }
    }
}
