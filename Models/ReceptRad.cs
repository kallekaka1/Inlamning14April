using System.ComponentModel.DataAnnotations.Schema;

namespace MorMorsBageruMVC.Models
{
    public class ReceptRad
    {
        [Column("recept_id")]
        public int ReceptId { get; set; }

        [Column("råvara_id")]
        public int RåvaraId { get; set; }

        [Column("mängd_kg")]
        public decimal MängdKg { get; set; }

        public Recept Recept { get; set; } = null!;
        public Råvara Råvara { get; set; } = null!;
    }
}
