namespace Inventory_List.Models
{
    public class Transaction
    {
        public int Id { get; set; }
        public int Type { get; set; }
        public DateTime Date { get; set; }
        public int? UserId { get; set; }
        public int? SupplierId { get; set; }
    }
}
