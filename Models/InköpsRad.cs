using System.ComponentModel.DataAnnotations.Schema;

namespace MorMorsBageruMVC.Models
{
    public class InköpsRad
    {
        [Column("inköp_id")]
        public int InköpId { get; set; }

        [Column("råvara_id")]
        public int RåvaraId { get; set; }

        [Column("mängd_kg")]
        public decimal MängdKg { get; set; }

        [Column("pris_per_kg")]
        public decimal PrisPerKg { get; set; }

        public Inköp Inköp { get; set; } = null!;
        public Råvara Råvara { get; set; } = null!;
    }
}
