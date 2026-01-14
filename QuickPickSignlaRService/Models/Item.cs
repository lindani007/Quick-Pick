namespace QuickPickSignlaRService.Models
{
    public class Item
    {
        public int ItemId { get; set; }
        public string? ItemName { get; set; }
        public double ItemPrice { get; set; }
        public string? ItemDescription { get; set; }
        public int ItemQuantity { get; set; }
        public string? ImageSource { get; set; }
        public int AisleId { get; set; }
    }
}
