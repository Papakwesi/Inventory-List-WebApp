namespace Inventory_List.Models
{
    public class TransactionDetail
    {
        public int Id { get; set; }
        public int TransactionId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public int UnitPrice { get; set; }
    }
}
