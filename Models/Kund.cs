using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MorMorsBageruMVC.Models
{
    public class Kund
    {
        [Key]
        [Column("id")]
        public int KundId { get; set; }

        [Required]
        [Column("butiksnamn")]
        public string Butiksnamn { get; set; } = "";

        [Column("telefon")]
        public string Telefon { get; set; } = "";

        [Column("epost")]
        public string Epost { get; set; } = "";

        [Column("kontaktperson")]
        public string Kontaktperson { get; set; } = "";

        [Column("leveransadress")]
        public string Leveransadress { get; set; } = "";

        [Column("fakturaadress")]
        public string Fakturaadress { get; set; } = "";

        public ICollection<Beställning> Beställningar { get; set; } = new List<Beställning>();
    }
}