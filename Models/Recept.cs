using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MorMorsBageruMVC.Models
{
    public class Recept
    {
        [Key]
        [Column("recept_id")]
        public int ReceptId { get; set; }

        [Column("produkt_id")]
        public int ProduktId { get; set; }

        public Produkt Produkt { get; set; } = null!;
        public ICollection<ReceptRad> Rader { get; set; } = new List<ReceptRad>();
    }
}
