using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MorMorsBageruMVC.Data;
using MorMorsBageruMVC.Models;

namespace MorMorsBageruMVC.Controllers
{
    public class ReceptController : Controller
    {
        private readonly AppDbContext _context;

        public ReceptController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var data = await _context.Recept
                .Include(r => r.Produkt)
                .Include(r => r.Rader)
                    .ThenInclude(rr => rr.Råvara)
                .ToListAsync();

            return View(data);
        }

        public async Task<IActionResult> Details(int id)
        {
            var model = await _context.Recept
                .Include(r => r.Produkt)
                .Include(r => r.Rader)
                    .ThenInclude(rr => rr.Råvara)
                .FirstOrDefaultAsync(r => r.ReceptId == id);

            if (model == null) return NotFound();
            return View(model);
        }

        public IActionResult Create() => View();

        [HttpPost]
        public async Task<IActionResult> Create(Recept model)
        {
            if (!ModelState.IsValid) return View(model);

            _context.Recept.Add(model);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var model = await _context.Recept.FindAsync(id);
            if (model == null) return NotFound();

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, Recept model)
        {
            if (id != model.ReceptId) return NotFound();
            if (!ModelState.IsValid) return View(model);

            _context.Update(model);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            var model = await _context.Recept.FirstOrDefaultAsync(r => r.ReceptId == id);
            if (model == null) return NotFound();

            return View(model);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var model = await _context.Recept.FindAsync(id);
            if (model != null)
            {
                _context.Recept.Remove(model);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
