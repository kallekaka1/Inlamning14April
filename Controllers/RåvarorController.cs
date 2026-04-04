using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MorMorsBageruMVC.Data;

namespace MorMorsBageruMVC.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RåvarorController : ControllerBase
    {
        private readonly AppDbContext _context;

        public RåvarorController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var ravaror = await _context.Råvaror
                .Include(r => r.LeverantörRåvaror)
                    .ThenInclude(lr => lr.Leverantör)
                .Select(r => new
                {
                    ravaraId = r.RåvaraId,
                    artikelnummer = r.Artikelnummer,
                    namn = r.Namn,
                    prisPerKg = r.PrisPerKg,
                    leverantorer = r.LeverantörRåvaror.Select(lr => new
                    {
                        leverantorId = lr.LeverantörId,
                        namn = lr.Leverantör.Namn,
                        adress = lr.Leverantör.Adress,
                        kontaktperson = lr.Leverantör.Kontaktperson,
                        telefon = lr.Leverantör.Telefon,
                        epost = lr.Leverantör.Epost,
                        pris = lr.Pris
                    })
                })
                .ToListAsync();

            return Ok(ravaror);
        }

        [HttpGet("search")]
        public async Task<IActionResult> Search([FromQuery] string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return BadRequest("Du måste ange ett namn att söka efter.");

            var ravara = await _context.Råvaror
                .Include(r => r.LeverantörRåvaror)
                    .ThenInclude(lr => lr.Leverantör)
                .Where(r => r.Namn.ToLower().Contains(name.ToLower()))
                .Select(r => new
                {
                    ravaraId = r.RåvaraId,
                    artikelnummer = r.Artikelnummer,
                    namn = r.Namn,
                    prisPerKg = r.PrisPerKg,
                    leverantorer = r.LeverantörRåvaror.Select(lr => new
                    {
                        leverantorId = lr.LeverantörId,
                        namn = lr.Leverantör.Namn,
                        adress = lr.Leverantör.Adress,
                        kontaktperson = lr.Leverantör.Kontaktperson,
                        telefon = lr.Leverantör.Telefon,
                        epost = lr.Leverantör.Epost,
                        pris = lr.Pris
                    })
                })
                .ToListAsync();

            if (!ravara.Any())
                return NotFound("Ingen råvara hittades.");

            return Ok(ravara);
        }
    }
}