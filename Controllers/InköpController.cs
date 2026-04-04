using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MorMorsBageruMVC.Data;
using MorMorsBageruMVC.Models;

namespace MorMorsBageruMVC.Controllers
{
    public class InköpController : Controller
    {
        private readonly AppDbContext _context;

        public InköpController(AppDbContext context)
        {
            _context = context;
        }

        // GET: /Inköp
        public async Task<IActionResult> Index()
        {
            var data = await _context.Inköp
                .Include(i => i.Leverantör)
                .Include(i => i.Rader)
                .ThenInclude(r => r.Råvara)
                .ToListAsync();

            return View(data);
        }

        // GET: /Inköp/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var inköp = await _context.Inköp
                .Include(i => i.Leverantör)
                .Include(i => i.Rader)
                    .ThenInclude(r => r.Råvara)
                .FirstOrDefaultAsync(i => i.InköpId == id);

            if (inköp == null) return NotFound();

            return View(inköp);
        }

        // GET: /Inköp/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: /Inköp/Create
        [HttpPost]
        public async Task<IActionResult> Create(Inköp inköp)
        {
            if (!ModelState.IsValid) return View(inköp);

            _context.Inköp.Add(inköp);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: /Inköp/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var inköp = await _context.Inköp.FindAsync(id);
            if (inköp == null) return NotFound();

            return View(inköp);
        }

        // POST: /Inköp/Edit/5
        [HttpPost]
        public async Task<IActionResult> Edit(int id, Inköp inköp)
        {
            if (id != inköp.InköpId) return NotFound();
            if (!ModelState.IsValid) return View(inköp);

            _context.Update(inköp);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // GET: /Inköp/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var inköp = await _context.Inköp
                .Include(i => i.Leverantör)
                .FirstOrDefaultAsync(i => i.InköpId == id);

            if (inköp == null) return NotFound();

            return View(inköp);
        }

        // POST: /Inköp/Delete/5
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var inköp = await _context.Inköp.FindAsync(id);
            if (inköp == null) return NotFound();

            _context.Inköp.Remove(inköp);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
