using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MorMorsBageruMVC.Data;
using MorMorsBageruMVC.Models;

namespace MorMorsBageruMVC.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrdersController : ControllerBase
    {
        private readonly AppDbContext _context;

        public OrdersController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrderDto>>> GetOrders()
        {
            var orders = await _context.Orders
                .Select(o => new OrderDto
                {
                    OrderId = o.OrderId,
                    OrderNumber = o.OrderNumber,
                    OrderDate = o.OrderDate,
                    CustomerId = o.CustomerId
                })
                .ToListAsync();

            return Ok(orders);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<OrderWithDetailsDto>> GetOrder(int id)
        {
            var order = await _context.Orders
                .Include(o => o.Customer)
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.BakeryProduct)
                .FirstOrDefaultAsync(o => o.OrderId == id);

            if (order == null)
                return NotFound("Order not found.");

            return Ok(new OrderWithDetailsDto
            {
                OrderId = order.OrderId,
                OrderNumber = order.OrderNumber,
                OrderDate = order.OrderDate,
                CustomerId = order.CustomerId,
                Customer = new OrderCustomerDto
                {
                    CustomerId = order.Customer!.CustomerId,
                    StoreName = order.Customer.StoreName,
                    ContactPerson = order.Customer.ContactPerson,
                    Email = order.Customer.Email
                },
                OrderItems = order.OrderItems.Select(oi => new OrderItemDetailDto
                {
                    ProductId = oi.BakeryProductId,
                    ProductName = oi.BakeryProduct!.ProductName,
                    Quantity = oi.Quantity,
                    UnitPrice = oi.UnitPrice,
                    TotalPrice = oi.TotalPrice
                }).ToList()
            });
        }

        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<OrderDto>>> SearchOrders(
            [FromQuery] string? orderNumber,
            [FromQuery] DateTime? orderDate)
        {
            if (string.IsNullOrWhiteSpace(orderNumber) && orderDate == null)
                return BadRequest("Use orderNumber or orderDate.");

            var query = _context.Orders.AsQueryable();

            if (!string.IsNullOrWhiteSpace(orderNumber))
            {
                query = query.Where(o => o.OrderNumber.Contains(orderNumber));
            }

            if (orderDate != null)
            {
                var startOfDay = orderDate.Value.Date;
                var endOfDay = startOfDay.AddDays(1);

                query = query.Where(o => o.OrderDate >= startOfDay && o.OrderDate < endOfDay);
            }

            var orders = await query
                .Select(o => new OrderDto
                {
                    OrderId = o.OrderId,
                    OrderNumber = o.OrderNumber,
                    OrderDate = o.OrderDate,
                    CustomerId = o.CustomerId
                })
                .ToListAsync();

            if (!orders.Any())
                return NotFound("No orders found.");

            return Ok(orders);
        }

        [HttpPost]
        public async Task<ActionResult<OrderDto>> CreateOrder([FromBody] CreateOrderDto request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var customerExists = await _context.Customers
                .AnyAsync(c => c.CustomerId == request.CustomerId);

            if (!customerExists)
                return BadRequest("Customer not found.");

            if (request.OrderItems.Count == 0)
                return BadRequest("Order must contain at least one product.");

            var productIds = request.OrderItems
                .Select(oi => oi.BakeryProductId)
                .Distinct()
                .ToList();

            var products = await _context.BakeryProducts
                .Where(p => productIds.Contains(p.BakeryProductId))
                .ToDictionaryAsync(p => p.BakeryProductId);

            foreach (var productId in productIds)
            {
                if (!products.ContainsKey(productId))
                    return BadRequest($"Product with ID {productId} not found.");
            }

            var order = new Order
            {
                OrderNumber = request.OrderNumber,
                OrderDate = request.OrderDate ?? DateTime.UtcNow,
                CustomerId = request.CustomerId
            };

            foreach (var item in request.OrderItems)
            {
                var totalPrice = item.Quantity * item.UnitPrice;

                order.OrderItems.Add(new OrderItem
                {
                    BakeryProductId = item.BakeryProductId,
                    Quantity = item.Quantity,
                    UnitPrice = item.UnitPrice,
                    TotalPrice = totalPrice
                });
            }

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetOrder), new { id = order.OrderId }, new OrderDto
            {
                OrderId = order.OrderId,
                OrderNumber = order.OrderNumber,
                OrderDate = order.OrderDate,
                CustomerId = order.CustomerId
            });
        }

        [HttpGet("customers-by-products")]
        public async Task<ActionResult<IEnumerable<ProductWithCustomersDto>>> GetCustomersByProducts()
        {
            var orderItems = await _context.OrderItems
                .Include(oi => oi.BakeryProduct)
                .Include(oi => oi.Order)
                    .ThenInclude(o => o.Customer)
                .ToListAsync();

            var result = orderItems
                .Where(oi => oi.BakeryProduct != null && oi.Order?.Customer != null)
                .GroupBy(oi => new
                {
                    oi.BakeryProductId,
                    oi.BakeryProduct!.ProductName
                })
                .Select(productGroup => new ProductWithCustomersDto
                {
                    ProductId = productGroup.Key.BakeryProductId,
                    ProductName = productGroup.Key.ProductName,
                    Customers = productGroup
                        .GroupBy(oi => new
                        {
                            oi.Order!.Customer!.CustomerId,
                            oi.Order.Customer.StoreName
                        })
                        .Select(customerGroup => new CustomerOrderInfoDto
                        {
                            CustomerId = customerGroup.Key.CustomerId,
                            StoreName = customerGroup.Key.StoreName,
                            OrderCount = customerGroup.Select(oi => oi.OrderId).Distinct().Count(),
                            TotalQuantity = customerGroup.Sum(oi => oi.Quantity),
                            TotalSpent = customerGroup.Sum(oi => oi.TotalPrice)
                        })
                        .ToList()
                })
                .ToList();

            return Ok(result);
        }

        public class OrderDto
        {
            public int OrderId { get; set; }
            public string OrderNumber { get; set; } = "";
            public DateTime OrderDate { get; set; }
            public int CustomerId { get; set; }
        }

        public class OrderWithDetailsDto : OrderDto
        {
            public OrderCustomerDto Customer { get; set; } = new();
            public List<OrderItemDetailDto> OrderItems { get; set; } = new();
        }

        public class OrderCustomerDto
        {
            public int CustomerId { get; set; }
            public string StoreName { get; set; } = "";
            public string ContactPerson { get; set; } = "";
            public string Email { get; set; } = "";
        }

        public class OrderItemDetailDto
        {
            public int ProductId { get; set; }
            public string ProductName { get; set; } = "";
            public int Quantity { get; set; }
            public decimal UnitPrice { get; set; }
            public decimal TotalPrice { get; set; }
        }

        public class CreateOrderDto
        {
            public string OrderNumber { get; set; } = "";
            public DateTime? OrderDate { get; set; }
            public int CustomerId { get; set; }
            public List<CreateOrderItemDto> OrderItems { get; set; } = new();
        }

        public class CreateOrderItemDto
        {
            public int BakeryProductId { get; set; }
            public int Quantity { get; set; }
            public decimal UnitPrice { get; set; }
        }

        public class ProductWithCustomersDto
        {
            public int ProductId { get; set; }
            public string ProductName { get; set; } = "";
            public List<CustomerOrderInfoDto> Customers { get; set; } = new();
        }

        public class CustomerOrderInfoDto
        {
            public int CustomerId { get; set; }
            public string StoreName { get; set; } = "";
            public int OrderCount { get; set; }
            public int TotalQuantity { get; set; }
            public decimal TotalSpent { get; set; }
        }
    }
}