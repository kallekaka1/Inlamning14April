using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MorMorsBageruMVC.Models
{
    public class Order
    {
        [Key]
        [Column("order_id")]
        public int OrderId { get; set; }

        [Required]
        [Column("order_number")]
        public string OrderNumber { get; set; } = "";

        [Required]
        [Column("order_date")]
        public DateTime OrderDate { get; set; } = DateTime.UtcNow;

        [Required]
        [Column("customer_id")]
        public int CustomerId { get; set; }

        public Customer? Customer { get; set; }

        public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
    }
}