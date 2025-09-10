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
        public int TotalTransactions { get; set; }
        // For "Products by Category" chart
        public IEnumerable<string> CategoryLabels { get; set; }
        public IEnumerable<int> CategoryCounts { get; set; }

        // For "Transactions Over Time" chart
        public IEnumerable<string> TransactionDates { get; set; }
        public IEnumerable<int> TransactionCounts { get; set; }

        public List<LowStockProductVM> LowStockProducts { get; set; }
    }

}
