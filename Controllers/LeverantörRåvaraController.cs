using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MorMorsBageruMVC.Data;
using MorMorsBageruMVC.Models;

namespace MorMorsBageruMVC.Controllers
{
    public class LeverantörRåvaraController : Controller
    {
        private readonly AppDbContext _context;

        public LeverantörRåvaraController(AppDbContext context)
        {
            _context = context;
        }

        // GET: /LeverantörRåvara
        public async Task<IActionResult> Index()
        {
            var data = await _context.LeverantörRåvaror
                .Include(lr => lr.Leverantör)
                .Include(lr => lr.Råvara)
                .ToListAsync();

            return View(data);
        }

        // GET: /LeverantörRåvara/Details/{leverantörId}/{råvaraId}
        public async Task<IActionResult> Details(int leverantörId, int råvaraId)
        {
            var model = await _context.LeverantörRåvaror
                .Include(lr => lr.Leverantör)
                .Include(lr => lr.Råvara)
                .FirstOrDefaultAsync(lr => lr.LeverantörId == leverantörId && lr.RåvaraId == råvaraId);

            if (model == null)
                return NotFound();

            return View(model);
        }

        // GET: /LeverantörRåvara/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: /LeverantörRåvara/Create
        [HttpPost]
        public async Task<IActionResult> Create(LeverantörRåvara model)
        {
            if (!ModelState.IsValid)
                return View(model);

            _context.LeverantörRåvaror.Add(model);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: /LeverantörRåvara/Delete/{leverantörId}/{råvaraId}
        public async Task<IActionResult> Delete(int leverantörId, int råvaraId)
        {
            var model = await _context.LeverantörRåvaror
                .Include(lr => lr.Leverantör)
                .Include(lr => lr.Råvara)
                .FirstOrDefaultAsync(lr => lr.LeverantörId == leverantörId && lr.RåvaraId == råvaraId);

            if (model == null)
                return NotFound();

            return View(model);
        }

        // POST: /LeverantörRåvara/Delete/{leverantörId}/{råvaraId}
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int leverantörId, int råvaraId)
        {
            var model = await _context.LeverantörRåvaror
                .FirstOrDefaultAsync(lr => lr.LeverantörId == leverantörId && lr.RåvaraId == råvaraId);

            if (model == null)
                return NotFound();

            _context.LeverantörRåvaror.Remove(model);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}
