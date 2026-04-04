using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MorMorsBageruMVC.Models
{
    public class Leverantör
    {
        [Key]
        [Column("leverantor_id")]
        public int LeverantörId { get; set; }

        [Column("namn")]
        public string Namn { get; set; } = "";

        [Column("adress")]
        public string Adress { get; set; } = "";

        [Column("kontaktperson")]
        public string Kontaktperson { get; set; } = "";

        [Column("telefon")]
        public string Telefon { get; set; } = "";

        [Column("epost")]
        public string Epost { get; set; } = "";

        public ICollection<Inköp> Inköp { get; set; } = new List<Inköp>();
        public ICollection<LeverantörRåvara> LeverantörRåvaror { get; set; } = new List<LeverantörRåvara>();
    }
}