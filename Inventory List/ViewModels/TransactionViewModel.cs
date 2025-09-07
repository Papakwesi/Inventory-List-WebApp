using Inventory_List.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace Inventory_List.ViewModels
{
    public class TransactionViewModel
    {
        public int Id { get; set; }
        public string Type { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        [ValidateNever]
        public IEnumerable<SelectListItem> Products { get; set; }
        [ValidateNever]
        public IEnumerable<SelectListItem> Types { get; set; }
    }
}
