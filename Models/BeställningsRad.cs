using System.ComponentModel.DataAnnotations.Schema;

namespace MorMorsBageruMVC.Models
{
    public class BeställningsRad
    {
        [Column("bestallning_id")]
        public int BeställningId { get; set; }

        public Beställning? Beställning { get; set; }

        [Column("produkt_id")]
        public int ProduktId { get; set; }

        public Produkt? Produkt { get; set; }

        [Column("antal")]
        public int Antal { get; set; }

        [Column("pris")]
        public decimal Pris { get; set; }

        [NotMapped]
        public decimal SummeratPris => Antal * Pris;
    }
}