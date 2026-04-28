using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MorMorsBageruMVC.Data;
using MorMorsBageruMVC.Models;

namespace MorMorsBageruMVC.Controllers
{
    [ApiController]
    [Route("api/bestallningar")]
    public class BeställningarController : ControllerBase
    {
        private readonly AppDbContext _context;

        public BeställningarController(AppDbContext context)
        {
            _context = context;
        }

        // POST: /api/bestallningar
        [HttpPost]
        public async Task<ActionResult<Beställning>> SkapaBeställning(Beställning beställning)
        {
            var kundFinns = await _context.Kunder.AnyAsync(k => k.KundId == beställning.KundId);

            if (!kundFinns)
                return BadRequest("Kunden finns inte.");

            foreach (var rad in beställning.Rader)
            {
                var produkt = await _context.Produkter.FindAsync(rad.ProduktId);

                if (produkt == null)
                    return BadRequest($"Produkt med id {rad.ProduktId} finns inte.");

                rad.Pris = produkt.PrisPerStyck;
            }

            _context.Beställningar.Add(beställning);
            await _context.SaveChangesAsync();

           return CreatedAtAction(nameof(HämtaBeställning), new { id = beställning.BeställningId }, new
{
    beställning.BeställningId,
    beställning.Beställningsnummer,
    beställning.Beställningsdatum,
    beställning.KundId,
    Rader = beställning.Rader.Select(r => new
    {
        r.ProduktId,
        r.Antal,
        r.Pris,
        SummeratPris = r.Antal * r.Pris
    })
});
        }

        // GET: /api/bestallningar
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Beställning>>> ListaBeställningar()
        {
            return await _context.Beställningar
                .Include(b => b.Kund)
                .Include(b => b.Rader)
                    .ThenInclude(r => r.Produkt)
                .ToListAsync();
        }

        // VG: GET /api/bestallningar/1
        [HttpGet("{id}")]
        public async Task<ActionResult<Beställning>> HämtaBeställning(int id)
        {
            var beställning = await _context.Beställningar
                .Include(b => b.Kund)
                .Include(b => b.Rader)
                    .ThenInclude(r => r.Produkt)
                .FirstOrDefaultAsync(b => b.BeställningId == id);

            if (beställning == null)
                return NotFound("Beställningen hittades inte.");

           return Ok(new
{
    beställning.BeställningId,
    beställning.Beställningsnummer,
    beställning.Beställningsdatum,
    Kund = new
    {
        beställning.Kund!.KundId,
        beställning.Kund.Butiksnamn,
        beställning.Kund.Kontaktperson
    },
    Rader = beställning.Rader.Select(r => new
    {
        r.ProduktId,
        ProduktNamn = r.Produkt!.Namn,
        r.Antal,
        r.Pris,
        SummeratPris = r.Antal * r.Pris
    })
});
        }

        // GET: /api/bestallningar/search?nummer=ORD-1001
        // GET: /api/bestallningar/search?datum=2026-04-28
        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<Beställning>>> SökBeställningar(
            [FromQuery] string? nummer,
            [FromQuery] DateTime? datum)
        {
            var query = _context.Beställningar
                .Include(b => b.Kund)
                .Include(b => b.Rader)
                    .ThenInclude(r => r.Produkt)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(nummer))
            {
                query = query.Where(b => b.Beställningsnummer.Contains(nummer));
            }

            if (datum.HasValue)
            {
                query = query.Where(b => b.Beställningsdatum.Date == datum.Value.Date);
            }

            return await query.ToListAsync();
        }
    }
}