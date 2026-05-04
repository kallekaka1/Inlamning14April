using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MorMorsBageruMVC.Data;
using MorMorsBageruMVC.Models;

namespace MorMorsBageruMVC.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SuppliersController : ControllerBase
    {
        private readonly AppDbContext _context;

        public SuppliersController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<SupplierDto>>> GetSuppliers()
        {
            var suppliers = await _context.Suppliers
                .Select(s => new SupplierDto
                {
                    SupplierId = s.SupplierId,
                    Name = s.Name,
                    Address = s.Address,
                    ContactPerson = s.ContactPerson,
                    Phone = s.Phone,
                    Email = s.Email
                })
                .ToListAsync();

            return Ok(suppliers);
        }

        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<SupplierWithIngredientsDto>>> SearchSuppliers([FromQuery] string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return BadRequest("Name is required.");

            var suppliers = await _context.Suppliers
                .Where(s => s.Name.ToLower().Contains(name.ToLower()))
                .Include(s => s.SupplierIngredients)
                    .ThenInclude(si => si.Ingredient)
                .Select(s => new SupplierWithIngredientsDto
                {
                    SupplierId = s.SupplierId,
                    Name = s.Name,
                    Address = s.Address,
                    ContactPerson = s.ContactPerson,
                    Phone = s.Phone,
                    Email = s.Email,
                    Ingredients = s.SupplierIngredients.Select(si => new SupplierIngredientDto
                    {
                        IngredientId = si.IngredientId,
                        ArticleNumber = si.Ingredient!.ArticleNumber,
                        Name = si.Ingredient.Name,
                        PricePerKg = si.PricePerKg
                    }).ToList()
                })
                .ToListAsync();

            if (!suppliers.Any())
                return NotFound("No suppliers found.");

            return Ok(suppliers);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<SupplierDto>> GetSupplier(int id)
        {
            var supplier = await _context.Suppliers.FindAsync(id);

            if (supplier == null)
                return NotFound("Supplier not found.");

            return Ok(new SupplierDto
            {
                SupplierId = supplier.SupplierId,
                Name = supplier.Name,
                Address = supplier.Address,
                ContactPerson = supplier.ContactPerson,
                Phone = supplier.Phone,
                Email = supplier.Email
            });
        }

        [HttpGet("{id:int}/ingredients")]
        public async Task<ActionResult<SupplierWithIngredientsDto>> GetSupplierWithIngredients(int id)
        {
            var supplier = await _context.Suppliers
                .Include(s => s.SupplierIngredients)
                    .ThenInclude(si => si.Ingredient)
                .FirstOrDefaultAsync(s => s.SupplierId == id);

            if (supplier == null)
                return NotFound("Supplier not found.");

            return Ok(new SupplierWithIngredientsDto
            {
                SupplierId = supplier.SupplierId,
                Name = supplier.Name,
                Address = supplier.Address,
                ContactPerson = supplier.ContactPerson,
                Phone = supplier.Phone,
                Email = supplier.Email,
                Ingredients = supplier.SupplierIngredients.Select(si => new SupplierIngredientDto
                {
                    IngredientId = si.IngredientId,
                    ArticleNumber = si.Ingredient!.ArticleNumber,
                    Name = si.Ingredient.Name,
                    PricePerKg = si.PricePerKg
                }).ToList()
            });
        }

        [HttpPost("{supplierId:int}/ingredients")]
        public async Task<IActionResult> AddIngredientToSupplier(
            int supplierId,
            [FromBody] AddIngredientToSupplierDto request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var supplier = await _context.Suppliers.FindAsync(supplierId);
            if (supplier == null)
                return NotFound("Supplier not found.");

            var ingredient = new Ingredient
            {
                ArticleNumber = request.ArticleNumber,
                Name = request.Name
            };

            _context.Ingredients.Add(ingredient);
            await _context.SaveChangesAsync();

            var supplierIngredient = new SupplierIngredient
            {
                SupplierId = supplierId,
                IngredientId = ingredient.IngredientId,
                PricePerKg = request.PricePerKg
            };

            _context.SupplierIngredients.Add(supplierIngredient);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetSupplierWithIngredients), new { id = supplierId }, new
            {
                ingredient.IngredientId,
                ingredient.ArticleNumber,
                ingredient.Name,
                supplierIngredient.PricePerKg
            });
        }

        [HttpPatch("{supplierId:int}/ingredients/{ingredientId:int}/price")]
        public async Task<IActionResult> UpdateIngredientPrice(
            int supplierId,
            int ingredientId,
            [FromBody] UpdatePriceDto request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var supplierIngredient = await _context.SupplierIngredients
                .FirstOrDefaultAsync(si => si.SupplierId == supplierId && si.IngredientId == ingredientId);

            if (supplierIngredient == null)
                return NotFound("Supplier-Ingredient association not found.");

            supplierIngredient.PricePerKg = request.PricePerKg;
            await _context.SaveChangesAsync();

            return Ok(new { message = "Price updated successfully." });
        }
    }

    public class SupplierDto
    {
        public int SupplierId { get; set; }
        public string Name { get; set; } = "";
        public string Address { get; set; } = "";
        public string ContactPerson { get; set; } = "";
        public string Phone { get; set; } = "";
        public string Email { get; set; } = "";
    }

    public class SupplierWithIngredientsDto
    {
        public int SupplierId { get; set; }
        public string Name { get; set; } = "";
        public string Address { get; set; } = "";
        public string ContactPerson { get; set; } = "";
        public string Phone { get; set; } = "";
        public string Email { get; set; } = "";
        public List<SupplierIngredientDto> Ingredients { get; set; } = new();
    }

    public class SupplierIngredientDto
    {
        public int IngredientId { get; set; }
        public string ArticleNumber { get; set; } = "";
        public string Name { get; set; } = "";
        public decimal PricePerKg { get; set; }
    }

    public class AddIngredientToSupplierDto
    {
        public string ArticleNumber { get; set; } = "";
        public string Name { get; set; } = "";
        public decimal PricePerKg { get; set; }
    }

    public class UpdatePriceDto
    {
        public decimal PricePerKg { get; set; }
    }
}