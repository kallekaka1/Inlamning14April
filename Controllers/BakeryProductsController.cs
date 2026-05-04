using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MorMorsBageruMVC.Data;
using MorMorsBageruMVC.Models;

namespace MorMorsBageruMVC.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BakeryProductsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public BakeryProductsController(AppDbContext context)
        {
            _context = context;
        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<BakeryProductDto>>> GetBakeryProducts()
        {
            var products = await _context.BakeryProducts
                .Select(p => new BakeryProductDto
                {
                    BakeryProductId = p.BakeryProductId,
                    ProductName = p.ProductName,
                    PricePerUnit = p.PricePerUnit,
                    Weight = p.Weight,
                    PackageQuantity = p.PackageQuantity,
                    ExpirationDate = p.ExpirationDate,
                    ManufacturingDate = p.ManufacturingDate
                })
                .ToListAsync();

            return Ok(products);
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<BakeryProductDto>> GetBakeryProduct(int id)
        {
            var product = await _context.BakeryProducts.FindAsync(id);

            if (product == null)
                return NotFound("Product not found.");

            return Ok(new BakeryProductDto
            {
                BakeryProductId = product.BakeryProductId,
                ProductName = product.ProductName,
                PricePerUnit = product.PricePerUnit,
                Weight = product.Weight,
                PackageQuantity = product.PackageQuantity,
                ExpirationDate = product.ExpirationDate,
                ManufacturingDate = product.ManufacturingDate
            });
        }


        [HttpPost]
        public async Task<ActionResult<BakeryProductDto>> CreateBakeryProduct([FromBody] CreateBakeryProductDto request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var product = new BakeryProduct
            {
                ProductName = request.ProductName,
                PricePerUnit = request.PricePerUnit,
                Weight = request.Weight,
                PackageQuantity = request.PackageQuantity,
                ExpirationDate = request.ExpirationDate,
                ManufacturingDate = request.ManufacturingDate
            };

            _context.BakeryProducts.Add(product);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetBakeryProduct), new { id = product.BakeryProductId }, new BakeryProductDto
            {
                BakeryProductId = product.BakeryProductId,
                ProductName = product.ProductName,
                PricePerUnit = product.PricePerUnit,
                Weight = product.Weight,
                PackageQuantity = product.PackageQuantity,
                ExpirationDate = product.ExpirationDate,
                ManufacturingDate = product.ManufacturingDate
            });
        }


        [HttpPatch("{id}/price")]
        public async Task<IActionResult> UpdatePrice(int id, [FromBody] UpdateBakeryProductPriceDto request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var product = await _context.BakeryProducts.FindAsync(id);
            if (product == null)
                return NotFound("Product not found.");

            product.PricePerUnit = request.PricePerUnit;
            await _context.SaveChangesAsync();

            return Ok(new { message = "Price updated successfully." });
        }
    }


    public class BakeryProductDto
    {
        public int BakeryProductId { get; set; }
        public string ProductName { get; set; }
        public decimal PricePerUnit { get; set; }
        public decimal Weight { get; set; }
        public int PackageQuantity { get; set; }
        public DateTime ExpirationDate { get; set; }
        public DateTime ManufacturingDate { get; set; }
    }

    public class CreateBakeryProductDto
    {
        public string ProductName { get; set; }
        public decimal PricePerUnit { get; set; }
        public decimal Weight { get; set; }
        public int PackageQuantity { get; set; }
        public DateTime ExpirationDate { get; set; }
        public DateTime ManufacturingDate { get; set; }
    }

    public class UpdateBakeryProductPriceDto
    {
        public decimal PricePerUnit { get; set; }
    }
}