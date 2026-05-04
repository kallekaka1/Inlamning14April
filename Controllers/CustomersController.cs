using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MorMorsBageruMVC.Data;
using MorMorsBageruMVC.Models;

namespace MorMorsBageruMVC.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CustomersController : ControllerBase
    {
        private readonly AppDbContext _context;

        public CustomersController(AppDbContext context)
        {
            _context = context;
        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<CustomerDto>>> GetCustomers()
        {
            var customers = await _context.Customers
                .Select(c => new CustomerDto
                {
                    CustomerId = c.CustomerId,
                    StoreName = c.StoreName,
                    Phone = c.Phone,
                    Email = c.Email,
                    ContactPerson = c.ContactPerson,
                    DeliveryAddress = c.DeliveryAddress,
                    InvoiceAddress = c.InvoiceAddress
                })
                .ToListAsync();

            return Ok(customers);
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<CustomerWithOrdersDto>> GetCustomer(int id)
        {
            var customer = await _context.Customers
                .Include(c => c.Orders)
                    .ThenInclude(o => o.OrderItems)
                        .ThenInclude(oi => oi.BakeryProduct)
                .FirstOrDefaultAsync(c => c.CustomerId == id);

            if (customer == null)
                return NotFound("Customer not found.");

            return Ok(new CustomerWithOrdersDto
            {
                CustomerId = customer.CustomerId,
                StoreName = customer.StoreName,
                Phone = customer.Phone,
                Email = customer.Email,
                ContactPerson = customer.ContactPerson,
                DeliveryAddress = customer.DeliveryAddress,
                InvoiceAddress = customer.InvoiceAddress,
                Orders = customer.Orders.Select(o => new OrderSummaryDto
                {
                    OrderId = o.OrderId,
                    OrderNumber = o.OrderNumber,
                    OrderDate = o.OrderDate,
                    OrderItems = o.OrderItems.Select(oi => new OrderItemSummaryDto
                    {
                        ProductId = oi.BakeryProductId,
                        ProductName = oi.BakeryProduct.ProductName,
                        Quantity = oi.Quantity,
                        UnitPrice = oi.UnitPrice,
                        TotalPrice = oi.TotalPrice
                    }).ToList()
                }).ToList()
            });
        }


        [HttpPost]
        public async Task<ActionResult<CustomerDto>> CreateCustomer([FromBody] CreateCustomerDto request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var customer = new Customer
            {
                StoreName = request.StoreName,
                Phone = request.Phone,
                Email = request.Email,
                ContactPerson = request.ContactPerson,
                DeliveryAddress = request.DeliveryAddress,
                InvoiceAddress = request.InvoiceAddress
            };

            _context.Customers.Add(customer);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetCustomer), new { id = customer.CustomerId }, new CustomerDto
            {
                CustomerId = customer.CustomerId,
                StoreName = customer.StoreName,
                Phone = customer.Phone,
                Email = customer.Email,
                ContactPerson = customer.ContactPerson,
                DeliveryAddress = customer.DeliveryAddress,
                InvoiceAddress = customer.InvoiceAddress
            });
        }


        [HttpPatch("{id}/contact-person")]
        public async Task<IActionResult> UpdateContactPerson(int id, [FromBody] UpdateContactPersonDto request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var customer = await _context.Customers.FindAsync(id);
            if (customer == null)
                return NotFound("Customer not found.");

            customer.ContactPerson = request.ContactPerson;
            await _context.SaveChangesAsync();

            return Ok(new { message = "Contact person updated successfully." });
        }
    }


    public class CustomerDto
    {
        public int CustomerId { get; set; }
        public string StoreName { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string ContactPerson { get; set; }
        public string DeliveryAddress { get; set; }
        public string InvoiceAddress { get; set; }
    }

    public class CustomerWithOrdersDto : CustomerDto
    {
        public List<OrderSummaryDto> Orders { get; set; } = new List<OrderSummaryDto>();
    }

    public class OrderSummaryDto
    {
        public int OrderId { get; set; }
        public string OrderNumber { get; set; }
        public DateTime OrderDate { get; set; }
        public List<OrderItemSummaryDto> OrderItems { get; set; } = new List<OrderItemSummaryDto>();
    }

    public class OrderItemSummaryDto
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TotalPrice { get; set; }
    }

    public class CreateCustomerDto
    {
        public string StoreName { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string ContactPerson { get; set; }
        public string DeliveryAddress { get; set; }
        public string InvoiceAddress { get; set; }
    }

    public class UpdateContactPersonDto
    {
        public string ContactPerson { get; set; }
    }
}