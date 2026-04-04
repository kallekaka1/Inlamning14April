using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MorMorsBageruMVC.Models
{
    public class Råvara
    {
        [Key]
        [Column("ravara_id")]
        public int RåvaraId { get; set; }

        [Column("artikelnummer")]
        public string Artikelnummer { get; set; } = "";

        [Column("namn")]
        public string Namn { get; set; } = "";

        [Column("pris_per_kg")]
        public decimal PrisPerKg { get; set; }

        public Lager Lager { get; set; } = null!;
        public ICollection<ReceptRad> ReceptRader { get; set; } = new List<ReceptRad>();
        public ICollection<InköpsRad> InköpsRader { get; set; } = new List<InköpsRad>();
        public ICollection<LeverantörRåvara> LeverantörRåvaror { get; set; } = new List<LeverantörRåvara>();
    }
}