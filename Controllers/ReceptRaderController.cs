using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MorMorsBageruMVC.Data;
using MorMorsBageruMVC.Models;

namespace MorMorsBageruMVC.Controllers
{
    public class ReceptRaderController : Controller
    {
        private readonly AppDbContext _context;

        public ReceptRaderController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var data = await _context.ReceptRader
                .Include(r => r.Recept)
                    .ThenInclude(rc => rc.Produkt)
                .Include(r => r.Råvara)
                .ToListAsync();

            return View(data);
        }

        public async Task<IActionResult> Details(int receptId, int råvaraId)
        {
            var model = await _context.ReceptRader
                .Include(r => r.Recept)
                    .ThenInclude(rc => rc.Produkt)
                .Include(r => r.Råvara)
                .FirstOrDefaultAsync(r => r.ReceptId == receptId && r.RåvaraId == råvaraId);

            if (model == null) return NotFound();
            return View(model);
        }

        public IActionResult Create() => View();

        [HttpPost]
        public async Task<IActionResult> Create(ReceptRad model)
        {
            if (!ModelState.IsValid) return View(model);

            _context.ReceptRader.Add(model);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int receptId, int råvaraId)
        {
            var model = await _context.ReceptRader.FindAsync(receptId, råvaraId);
            if (model == null) return NotFound();

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int receptId, int råvaraId, ReceptRad model)
        {
            if (receptId != model.ReceptId || råvaraId != model.RåvaraId)
                return NotFound();

            if (!ModelState.IsValid) return View(model);

            _context.Update(model);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int receptId, int råvaraId)
        {
            var model = await _context.ReceptRader
                .Include(r => r.Recept)
                .Include(r => r.Råvara)
                .FirstOrDefaultAsync(r => r.ReceptId == receptId && r.RåvaraId == råvaraId);

            if (model == null) return NotFound();

            return View(model);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int receptId, int råvaraId)
        {
            var model = await _context.ReceptRader.FindAsync(receptId, råvaraId);
            if (model != null)
            {
                _context.ReceptRader.Remove(model);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
