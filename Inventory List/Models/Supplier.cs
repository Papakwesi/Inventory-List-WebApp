using System.ComponentModel.DataAnnotations;

namespace Inventory_List.Models
{
    public class Supplier
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string ContactPerson { get; set; }
        [Required]
        public int PhoneNumber { get; set; }
        [Required]
        public string HomeAddress { get; set; }
        public string City { get; set; }
        public string Region { get; set; }
        public string PostalCode { get; set; }
        public string Country { get; set; }
        [Required]
        public string Email { get; set; }
    }
}
