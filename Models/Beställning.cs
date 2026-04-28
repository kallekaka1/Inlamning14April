using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MorMorsBageruMVC.Models
{
    public class Beställning
    {
        [Key]
        [Column("id")]
        public int BeställningId { get; set; }

        [Required]
        [Column("bestallningsnummer")]
        public string Beställningsnummer { get; set; } = "";

        [Column("bestallningsdatum")]
        public DateTime Beställningsdatum { get; set; } = DateTime.Now;

        [Column("kund_id")]
        public int KundId { get; set; }

        public Kund? Kund { get; set; }

        public ICollection<BeställningsRad> Rader { get; set; } = new List<BeställningsRad>();
    }
}