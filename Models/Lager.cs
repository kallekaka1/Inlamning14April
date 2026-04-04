using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MorMorsBageruMVC.Models
{
    public class Lager
    {
        [Key]
        [Column("råvara_id")]
        public int RåvaraId { get; set; }

        [Column("mängd_kg")]
        public decimal MängdKg { get; set; }

        public Råvara Råvara { get; set; } = null!;
    }
}
