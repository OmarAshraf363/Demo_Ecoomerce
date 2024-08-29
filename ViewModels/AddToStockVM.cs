using Demo.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace Demo.ViewModels
{
    public class AddToStockVM
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public int StoreId { get; set; }
        [ValidateNever]
        public List<Store> Stores { get; set; } = new List<Store>();
    }
}
