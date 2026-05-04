using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MorMorsBageruMVC.Models
{
    public class Ingredient
    {
        [Key]
        [Column("ingredient_id")]
        public int IngredientId { get; set; }

        [Required]
        [Column("article_number")]
        public string ArticleNumber { get; set; } = "";

        [Required]
        [Column("name")]
        public string Name { get; set; } = "";

        public ICollection<SupplierIngredient> SupplierIngredients { get; set; } = new List<SupplierIngredient>();
    }
}