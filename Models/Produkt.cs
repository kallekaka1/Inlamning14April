using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MorMorsBageruMVC.Models
{
    public class Produkt
    {
        [Key]
        [Column("id")]
        public int ProduktId { get; set; }

        [Required]
        [Column("namn")]
        public string Namn { get; set; } = "";

        [Column("kategori")]
        public string? Kategori { get; set; }

        [Column("pris_per_styck")]
        public decimal PrisPerStyck { get; set; }

        [Column("vikt")]
        public decimal Vikt { get; set; }

        [Column("antal_i_forpackning")]
        public int AntalIFörpackning { get; set; }

        [Column("bast_fore_datum")]
        public DateTime BästFöreDatum { get; set; }

        [Column("tillverkningsdatum")]
        public DateTime Tillverkningsdatum { get; set; }

        public Recept? Recept { get; set; }

        public ICollection<BeställningsRad> BeställningsRader { get; set; } = new List<BeställningsRad>();
    }
}