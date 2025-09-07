using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace Inventory_List.Models
{
    public class LowStockProductVM
    {
        public string Name { get; set; }
        public int Quantity { get; set; }
        public int ReorderLevel { get; set; }
    }
}
