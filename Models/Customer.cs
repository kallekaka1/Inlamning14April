using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MorMorsBageruMVC.Models
{
    public class Customer
    {
        [Key]
        [Column("customer_id")]
        public int CustomerId { get; set; }

        [Required]
        [Column("store_name")]
        public string StoreName { get; set; } = "";

        [Required]
        [Column("phone")]
        public string Phone { get; set; } = "";

        [Required]
        [Column("email")]
        public string Email { get; set; } = "";

        [Required]
        [Column("contact_person")]
        public string ContactPerson { get; set; } = "";

        [Required]
        [Column("delivery_address")]
        public string DeliveryAddress { get; set; } = "";

        [Required]
        [Column("invoice_address")]
        public string InvoiceAddress { get; set; } = "";

        public ICollection<Order> Orders { get; set; } = new List<Order>();
    }
}