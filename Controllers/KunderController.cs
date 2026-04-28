using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MorMorsBageruMVC.Data;
using MorMorsBageruMVC.Models;

namespace MorMorsBageruMVC.Controllers
{
    [ApiController]
    [Route("api/kunder")]
    public class KunderController : ControllerBase
    {
        private readonly AppDbContext _context;

        public KunderController(AppDbContext context)
        {
            _context = context;
        }

        // POST: /api/kunder
        [HttpPost]
        public async Task<ActionResult<Kund>> SkapaKund(Kund kund)
        {
            _context.Kunder.Add(kund);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(HämtaKund), new { id = kund.KundId }, kund);
        }

        // GET: /api/kunder
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Kund>>> ListaKunder()
        {
            return await _context.Kunder
                .Include(k => k.Beställningar)
                .ToListAsync();
        }

        // GET: /api/kunder/1
        [HttpGet("{id}")]
        public async Task<ActionResult<Kund>> HämtaKund(int id)
        {
            var kund = await _context.Kunder
                .Include(k => k.Beställningar)
                    .ThenInclude(b => b.Rader)
                        .ThenInclude(r => r.Produkt)
                .FirstOrDefaultAsync(k => k.KundId == id);

            if (kund == null)
                return NotFound("Kunden hittades inte.");

            return kund;
        }

        // VG: PUT /api/kunder/1/kontaktperson
        [HttpPut("{id}/kontaktperson")]
        public async Task<IActionResult> UppdateraKontaktperson(int id, [FromBody] UppdateraKontaktpersonRequest request)
        {
            var kund = await _context.Kunder.FindAsync(id);

            if (kund == null)
                return NotFound("Kunden hittades inte.");

            kund.Kontaktperson = request.Kontaktperson;

            await _context.SaveChangesAsync();

            return Ok(kund);
        }

        // VG: GET /api/kunder/produkt/1
        [HttpGet("produkt/{produktId}")]
        public async Task<IActionResult> KunderSomKöptProdukt(int produktId)
        {
            var kunder = await _context.BeställningsRader
                .Where(r => r.ProduktId == produktId)
                .Include(r => r.Beställning)
                    .ThenInclude(b => b!.Kund)
                .Select(r => r.Beställning!.Kund)
                .Distinct()
                .ToListAsync();

            return Ok(kunder);
        }
    }

    public class UppdateraKontaktpersonRequest
    {
        public string Kontaktperson { get; set; } = "";
    }
}