using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MorMorsBageruMVC.Data;
using MorMorsBageruMVC.Models;

namespace MorMorsBageruMVC.Controllers
{
    public class InköpsRaderController : Controller
    {
        private readonly AppDbContext _context;

        public InköpsRaderController(AppDbContext context)
        {
            _context = context;
        }

        // GET: /InköpsRader
        public async Task<IActionResult> Index()
        {
            var data = await _context.InköpsRader
                .Include(r => r.Inköp)
                    .ThenInclude(i => i.Leverantör)
                .Include(r => r.Råvara)
                .ToListAsync();

            return View(data);
        }

        // GET: /InköpsRader/Details/{inköpId}/{råvaraId}
        public async Task<IActionResult> Details(int inköpId, int råvaraId)
        {
            var rad = await _context.InköpsRader
                .Include(r => r.Inköp)
                    .ThenInclude(i => i.Leverantör)
                .Include(r => r.Råvara)
                .FirstOrDefaultAsync(r => r.InköpId == inköpId && r.RåvaraId == råvaraId);

            if (rad == null) return NotFound();

            return View(rad);
        }

        // GET: /InköpsRader/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: /InköpsRader/Create
        [HttpPost]
        public async Task<IActionResult> Create(InköpsRad rad)
        {
            if (!ModelState.IsValid)
                return View(rad);

            _context.InköpsRader.Add(rad);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: /InköpsRader/Edit/{inköpId}/{råvaraId}
        public async Task<IActionResult> Edit(int inköpId, int råvaraId)
        {
            var rad = await _context.InköpsRader.FindAsync(inköpId, råvaraId);
            if (rad == null) return NotFound();

            return View(rad);
        }

        // POST: /InköpsRader/Edit/{inköpId}/{råvaraId}
        [HttpPost]
        public async Task<IActionResult> Edit(int inköpId, int råvaraId, InköpsRad rad)
        {
            if (inköpId != rad.InköpId || råvaraId != rad.RåvaraId)
                return NotFound();

            if (!ModelState.IsValid)
                return View(rad);

            _context.Update(rad);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // GET: /InköpsRader/Delete/{inköpId}/{råvaraId}
        public async Task<IActionResult> Delete(int inköpId, int råvaraId)
        {
            var rad = await _context.InköpsRader
                .Include(r => r.Inköp)
                    .ThenInclude(i => i.Leverantör)
                .Include(r => r.Råvara)
                .FirstOrDefaultAsync(r => r.InköpId == inköpId && r.RåvaraId == råvaraId);

            if (rad == null) return NotFound();

            return View(rad);
        }

        // POST: /InköpsRader/Delete/{inköpId}/{råvaraId}
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int inköpId, int råvaraId)
        {
            var rad = await _context.InköpsRader.FindAsync(inköpId, råvaraId);
            if (rad == null) return NotFound();

            _context.InköpsRader.Remove(rad);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}
