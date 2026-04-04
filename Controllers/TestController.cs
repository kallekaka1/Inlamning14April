using Microsoft.AspNetCore.Mvc;
using MorMorsBageruMVC.Data;

namespace MorMorsBageruMVC.Controllers
{
    public class TestController : Controller
    {
        private readonly AppDbContext _db;

        public TestController(AppDbContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            int count = _db.Leverantörer.Count();
            return Content("Antal leverantörer i DB: " + count);
        }
    }
}
