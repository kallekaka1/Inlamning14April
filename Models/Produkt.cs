using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MorMorsBageruMVC.Models
{
    public class Produkt
    {
        [Key]
        [Column("id")]                      // PK i tabellen produkter
        public int ProduktId { get; set; }

        [Column("namn")]
        public string Namn { get; set; } = "";

        [Column("kategori")]
        public string? Kategori { get; set; }

        // En produkt kan ha 0 eller 1 recept
        public Recept? Recept { get; set; }
    }
}
