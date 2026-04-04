using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MorMorsBageruMVC.Models
{
    public class Inköp
    {
        [Key]
        [Column("inköp_id")]
        public int InköpId { get; set; }

        [Column("datum")]
        public DateTime Datum { get; set; }

        [Column("leverantör_id")]
        public int LeverantörId { get; set; }

        public Leverantör Leverantör { get; set; } = null!;
        public ICollection<InköpsRad> Rader { get; set; } = new List<InköpsRad>();
    }
}
