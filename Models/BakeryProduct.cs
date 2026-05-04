using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MorMorsBageruMVC.Models
{
    public class BakeryProduct
    {
        [Key]
        [Column("product_id")]
        public int BakeryProductId { get; set; }

        [Required]
        [Column("product_name")]
        public string ProductName { get; set; } = "";

        [Required]
        [Column("price_per_unit")]
        public decimal PricePerUnit { get; set; }

        [Required]
        [Column("weight")]
        public decimal Weight { get; set; }

        [Required]
        [Column("package_quantity")]
        public int PackageQuantity { get; set; }

        [Required]
        [Column("expiration_date")]
        public DateTime ExpirationDate { get; set; }

        [Required]
        [Column("manufacturing_date")]
        public DateTime ManufacturingDate { get; set; }

        public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
    }
}