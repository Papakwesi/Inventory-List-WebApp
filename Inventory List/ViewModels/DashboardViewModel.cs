using Inventory_List.Models;

namespace Inventory_List.ViewModels
{
    public class DashboardViewModel
    {
        public int TotalCategories { get; set; }
        public int TotalProducts { get; set; }
        public int TotalSuppliers { get; set; }
        public int TotalCustomers { get; set; }
        public int ReorderCount { get; set; }
        public List<LowStockProductVM> LowStockProducts { get; set; }
    }

}
