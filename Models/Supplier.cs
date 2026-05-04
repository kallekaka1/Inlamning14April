using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MorMorsBageruMVC.Models
{
    public class Supplier
    {
        [Key]
        [Column("supplier_id")]
        public int SupplierId { get; set; }

        [Required]
        [Column("name")]
        public string Name { get; set; } = "";

        [Column("address")]
        public string Address { get; set; } = "";

        [Column("contact_person")]
        public string ContactPerson { get; set; } = "";

        [Column("phone")]
        public string Phone { get; set; } = "";

        [Column("email")]
        public string Email { get; set; } = "";

        public ICollection<SupplierIngredient> SupplierIngredients { get; set; } = new List<SupplierIngredient>();
    }
}