using System.ComponentModel.DataAnnotations.Schema;

namespace MorMorsBageruMVC.Models
{
    public class LeverantörRåvara
    {
        [Column("leverantor_id")]
        public int LeverantörId { get; set; }

        [Column("ravara_id")]
        public int RåvaraId { get; set; }

        [Column("pris")]
        public decimal Pris { get; set; }

        public Leverantör Leverantör { get; set; } = null!;
        public Råvara Råvara { get; set; } = null!;
    }
}