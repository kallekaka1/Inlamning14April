using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MorMorsBageruMVC.Data;
using MorMorsBageruMVC.Models;

namespace MorMorsBageruMVC.Controllers
{
    public class ProdukterController : Controller
    {
        private readonly AppDbContext _context;

        public ProdukterController(AppDbContext context)
        {
            _context = context;
        }

        // -----------------------------
        // LISTA ALLA PRODUKTER
        // -----------------------------
        public async Task<IActionResult> Index()
        {
            var data = await _context.Produkter
                .Include(p => p.Recept)
                .ThenInclude(r => r.Rader)
                .ToListAsync();

            return View(data);
        }

        // -----------------------------
        // DETALJVY FÖR EN PRODUKT
        // -----------------------------
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var produkt = await _context.Produkter
                .Include(p => p.Recept)
                    .ThenInclude(r => r.Rader)
                .FirstOrDefaultAsync(m => m.ProduktId == id);

            if (produkt == null)
                return NotFound();

            return View(produkt);
        }
    }
}
