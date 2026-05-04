using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MorMorsBageruMVC.Models
{
    public class OrderItem
    {
        [Column("order_id")]
        [Required]
        public int OrderId { get; set; }

        [Column("product_id")]
        [Required]
        public int BakeryProductId { get; set; }

        [Column("quantity")]
        [Required]
        public int Quantity { get; set; }

        [Column("unit_price")]
        [Required]
        public decimal UnitPrice { get; set; }

        [Column("total_price")]
        [Required]
        public decimal TotalPrice { get; set; }

        public Order? Order { get; set; }
        public BakeryProduct? BakeryProduct { get; set; }
    }
}