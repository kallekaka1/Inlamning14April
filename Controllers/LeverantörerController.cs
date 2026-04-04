using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MorMorsBageruMVC.Data;
using MorMorsBageruMVC.Models;

namespace MorMorsBageruMVC.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LeverantörerController : ControllerBase
    {
        private readonly AppDbContext _context;

        public LeverantörerController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("search")]
        public async Task<IActionResult> Search([FromQuery] string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return BadRequest("Du måste ange ett namn att söka efter.");

            var leverantorer = await _context.Leverantörer
                .Include(l => l.LeverantörRåvaror)
                    .ThenInclude(lr => lr.Råvara)
                .Where(l => l.Namn.ToLower().Contains(name.ToLower()))
                .Select(l => new
                {
                    leverantorId = l.LeverantörId,
                    namn = l.Namn,
                    adress = l.Adress,
                    kontaktperson = l.Kontaktperson,
                    telefon = l.Telefon,
                    epost = l.Epost,
                    produkter = l.LeverantörRåvaror.Select(lr => new
                    {
                        ravaraId = lr.RåvaraId,
                        artikelnummer = lr.Råvara.Artikelnummer,
                        namn = lr.Råvara.Namn,
                        prisPerKg = lr.Råvara.PrisPerKg,
                        leverantorsPris = lr.Pris
                    })
                })
                .ToListAsync();

            if (!leverantorer.Any())
                return NotFound("Ingen leverantör hittades.");

            return Ok(leverantorer);
        }

        [HttpPost("{leverantorId}/produkter")]
        public async Task<IActionResult> AddProductToSupplier(int leverantorId, [FromBody] AddProductToSupplierRequest request)
        {
            var leverantor = await _context.Leverantörer.FindAsync(leverantorId);
            if (leverantor == null)
                return NotFound("Leverantören hittades inte.");

            var ravara = await _context.Råvaror.FindAsync(request.RåvaraId);
            if (ravara == null)
                return NotFound("Råvaran hittades inte.");

            var exists = await _context.LeverantörRåvaror.AnyAsync(lr =>
                lr.LeverantörId == leverantorId && lr.RåvaraId == request.RåvaraId);

            if (exists)
                return BadRequest("Denna råvara finns redan kopplad till leverantören.");

            var leverantorRavara = new LeverantörRåvara
            {
                LeverantörId = leverantorId,
                RåvaraId = request.RåvaraId,
                Pris = request.Pris
            };

            _context.LeverantörRåvaror.Add(leverantorRavara);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                message = "Produkten kopplades till leverantören."
            });
        }

        [HttpPut("{leverantorId}/ravara/{ravaraId}/pris")]
        public async Task<IActionResult> UpdatePrice(int leverantorId, int ravaraId, [FromBody] UpdatePriceRequest request)
        {
            var leverantorRavara = await _context.LeverantörRåvaror
                .FirstOrDefaultAsync(lr => lr.LeverantörId == leverantorId && lr.RåvaraId == ravaraId);

            if (leverantorRavara == null)
                return NotFound("Kopplingen mellan leverantör och råvara hittades inte.");

            leverantorRavara.Pris = request.Pris;
            await _context.SaveChangesAsync();

            return Ok(new
            {
                message = "Priset uppdaterades."
            });
        }

        public class AddProductToSupplierRequest
        {
            public int RåvaraId { get; set; }
            public decimal Pris { get; set; }
        }

        public class UpdatePriceRequest
        {
            public decimal Pris { get; set; }
        }
    }
}