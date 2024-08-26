namespace Demo.ViewModels
{
    public class CartViewModel
    {
        public int? OrderId { get; set; }
        public int ProductId { get; set; }
        public string? ProductName { get; set; }
        public string ProductDescription { get; set; } = string.Empty;
        public string? Img {  get; set; }
        public int? Quantity { get; set; }
        public decimal? ListPrice { get; set; }
        public decimal? TotalPrice { get; set; }
    }
}
