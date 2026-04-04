using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MorMorsBageruMVC.Data;
using MorMorsBageruMVC.Models;

namespace MorMorsBageruMVC.Controllers
{
    public class LagerController : Controller
    {
        private readonly AppDbContext _context;

        public LagerController(AppDbContext context)
        {
            _context = context;
        }

        // GET: /Lager
        public async Task<IActionResult> Index()
        {
            var data = await _context.Lager
                .Include(l => l.Råvara)
                .ToListAsync();

            return View(data);
        }

        // GET: /Lager/Details/{råvaraId}
        public async Task<IActionResult> Details(int id)
        {
            var data = await _context.Lager
                .Include(l => l.Råvara)
                .FirstOrDefaultAsync(l => l.RåvaraId == id);

            if (data == null) return NotFound();

            return View(data);
        }

        // GET: /Lager/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: /Lager/Create
        [HttpPost]
        public async Task<IActionResult> Create(Lager lager)
        {
            if (!ModelState.IsValid)
                return View(lager);

            _context.Lager.Add(lager);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: /Lager/Edit/{råvaraId}
        public async Task<IActionResult> Edit(int id)
        {
            var data = await _context.Lager.FindAsync(id);

            if (data == null) return NotFound();

            return View(data);
        }

        // POST: /Lager/Edit/{råvaraId}
        [HttpPost]
        public async Task<IActionResult> Edit(int id, Lager lager)
        {
            if (id != lager.RåvaraId)
                return NotFound();

            if (!ModelState.IsValid)
                return View(lager);

            _context.Update(lager);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // GET: /Lager/Delete/{råvaraId}
        public async Task<IActionResult> Delete(int id)
        {
            var data = await _context.Lager
                .Include(l => l.Råvara)
                .FirstOrDefaultAsync(l => l.RåvaraId == id);

            if (data == null) return NotFound();

            return View(data);
        }

        // POST: /Lager/Delete/{råvaraId}
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var data = await _context.Lager.FindAsync(id);
            if (data == null) return NotFound();

            _context.Lager.Remove(data);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
