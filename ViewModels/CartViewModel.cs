namespace Demo.ViewModels
{
    public class CartViewModel
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public int? Quantity { get; set; }
        public decimal? ListPrice { get; set; }
        public decimal? TotalPrice { get; set; }
    }
}
