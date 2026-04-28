using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MorMorsBageruMVC.Data;
using MorMorsBageruMVC.Models;

namespace MorMorsBageruMVC.Controllers
{
    [ApiController]
    [Route("api/produkter")]
    public class ProdukterApiController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ProdukterApiController(AppDbContext context)
        {
            _context = context;
        }

        // POST: /api/produkter
        [HttpPost]
        public async Task<ActionResult<Produkt>> SkapaProdukt(Produkt produkt)
        {
            _context.Produkter.Add(produkt);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(HämtaProdukt), new { id = produkt.ProduktId }, produkt);
        }

        // GET: /api/produkter
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Produkt>>> ListaProdukter()
        {
            return await _context.Produkter.ToListAsync();
        }

        // GET: /api/produkter/1
        [HttpGet("{id}")]
        public async Task<ActionResult<Produkt>> HämtaProdukt(int id)
        {
            var produkt = await _context.Produkter
                .Include(p => p.BeställningsRader)
                .FirstOrDefaultAsync(p => p.ProduktId == id);

            if (produkt == null)
                return NotFound("Produkten hittades inte.");

            return produkt;
        }

        // PUT: /api/produkter/1/pris
        [HttpPut("{id}/pris")]
        public async Task<IActionResult> UppdateraPris(int id, [FromBody] UppdateraPrisRequest request)
        {
            var produkt = await _context.Produkter.FindAsync(id);

            if (produkt == null)
                return NotFound("Produkten hittades inte.");

            produkt.PrisPerStyck = request.PrisPerStyck;

            await _context.SaveChangesAsync();

            return Ok(produkt);
        }
    }

    public class UppdateraPrisRequest
    {
        public decimal PrisPerStyck { get; set; }
    }
}