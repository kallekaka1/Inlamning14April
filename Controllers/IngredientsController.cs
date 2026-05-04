using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MorMorsBageruMVC.Data;
using MorMorsBageruMVC.Models;

namespace MorMorsBageruMVC.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class IngredientsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public IngredientsController(AppDbContext context)
        {
            _context = context;
        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<IngredientWithSuppliersDto>>> GetIngredients()
        {
            var ingredients = await _context.Ingredients
                .Include(i => i.SupplierIngredients)
                    .ThenInclude(si => si.Supplier)
                .Select(i => new IngredientWithSuppliersDto
                {
                    IngredientId = i.IngredientId,
                    ArticleNumber = i.ArticleNumber,
                    Name = i.Name,
                    Suppliers = i.SupplierIngredients.Select(si => new SupplierWithPriceDto
                    {
                        SupplierId = si.SupplierId,
                        SupplierName = si.Supplier.Name,
                        Address = si.Supplier.Address,
                        Phone = si.Supplier.Phone,
                        Email = si.Supplier.Email,
                        PricePerKg = si.PricePerKg
                    }).ToList()
                })
                .ToListAsync();

            return Ok(ingredients);
        }


        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<IngredientWithSuppliersDto>>> SearchIngredients([FromQuery] string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return BadRequest("Search term is required.");

            var ingredients = await _context.Ingredients
                .Where(i => i.Name.ToLower().Contains(name.ToLower()))
                .Include(i => i.SupplierIngredients)
                    .ThenInclude(si => si.Supplier)
                .Select(i => new IngredientWithSuppliersDto
                {
                    IngredientId = i.IngredientId,
                    ArticleNumber = i.ArticleNumber,
                    Name = i.Name,
                    Suppliers = i.SupplierIngredients.Select(si => new SupplierWithPriceDto
                    {
                        SupplierId = si.SupplierId,
                        SupplierName = si.Supplier.Name,
                        Address = si.Supplier.Address,
                        Phone = si.Supplier.Phone,
                        Email = si.Supplier.Email,
                        PricePerKg = si.PricePerKg
                    }).ToList()
                })
                .ToListAsync();

            if (!ingredients.Any())
                return NotFound("No ingredients found matching your search.");

            return Ok(ingredients);
        }
    }

    // DTOs for Ingredients
    public class IngredientWithSuppliersDto
    {
        public int IngredientId { get; set; }
        public string ArticleNumber { get; set; }
        public string Name { get; set; }
        public List<SupplierWithPriceDto> Suppliers { get; set; } = new List<SupplierWithPriceDto>();
    }

    public class SupplierWithPriceDto
    {
        public int SupplierId { get; set; }
        public string SupplierName { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public decimal PricePerKg { get; set; }
    }
}