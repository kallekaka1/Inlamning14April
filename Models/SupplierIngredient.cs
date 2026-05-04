using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MorMorsBageruMVC.Models
{
    public class SupplierIngredient
    {
        [Column("supplier_id")]
        public int SupplierId { get; set; }

        [Column("ingredient_id")]
        public int IngredientId { get; set; }

        [Column("price_per_kg")]
        [Required]
        public decimal PricePerKg { get; set; }

        public Supplier? Supplier { get; set; }
        public Ingredient? Ingredient { get; set; }
    }
}