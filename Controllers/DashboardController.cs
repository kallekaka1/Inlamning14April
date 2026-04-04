using Microsoft.AspNetCore.Mvc;
using MorMorsBageruMVC.Data;
using System.Linq;

namespace MorMorsBageruMVC.Controllers
{
    public class DashboardController : Controller
    {
        private readonly AppDbContext _db;

        public DashboardController(AppDbContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            var model = new
            {
                Leverantörer = _db.Leverantörer.Count(),
                Råvaror = _db.Råvaror.Count(),
                Produkter = _db.Produkter.Count(),
                Recept = _db.Recept.Count(),
                ReceptRader = _db.ReceptRader.Count(),
                Lager = _db.Lager.Count(),
                Inköp = _db.Inköp.Count(),
                InköpsRader = _db.InköpsRader.Count(),
                LeverantörRåvara = _db.LeverantörRåvaror.Count()
            };

            return View(model);
        }
    }
}
