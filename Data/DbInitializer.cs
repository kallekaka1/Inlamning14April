using MorMorsBageruMVC.Models;

namespace MorMorsBageruMVC.Data
{
    public static class DbInitializer
    {
        public static void Initialize(AppDbContext context)
        {
            context.Database.EnsureCreated();

            if (
                context.Suppliers.Any() ||
                context.Ingredients.Any() ||
                context.SupplierIngredients.Any() ||
                context.Customers.Any() ||
                context.BakeryProducts.Any() ||
                context.Orders.Any() ||
                context.OrderItems.Any()
            )
            {
                return;
            }

            var suppliers = new List<Supplier>
            {
                new Supplier
                {
                    Name = "Premium Flour Mills",
                    Address = "Stockholm, Sweden",
                    ContactPerson = "Anna Andersson",
                    Phone = "+46701234567",
                    Email = "anna@premiumflour.se"
                },
                new Supplier
                {
                    Name = "Nordic Butter Co.",
                    Address = "Gothenburg, Sweden",
                    ContactPerson = "Erik Eriksson",
                    Phone = "+46707654321",
                    Email = "erik@nordicbutter.se"
                },
                new Supplier
                {
                    Name = "Sweet Ingredients Ltd.",
                    Address = "Malmö, Sweden",
                    ContactPerson = "Sofia Svensson",
                    Phone = "+46709876543",
                    Email = "sofia@sweetingredients.se"
                }
            };

            context.Suppliers.AddRange(suppliers);
            context.SaveChanges();

            var ingredients = new List<Ingredient>
            {
                new Ingredient { ArticleNumber = "ING001", Name = "Wheat Flour" },
                new Ingredient { ArticleNumber = "ING002", Name = "Butter" },
                new Ingredient { ArticleNumber = "ING003", Name = "Sugar" },
                new Ingredient { ArticleNumber = "ING004", Name = "Eggs" },
                new Ingredient { ArticleNumber = "ING005", Name = "Vanilla Extract" },
                new Ingredient { ArticleNumber = "ING006", Name = "Baking Powder" },
                new Ingredient { ArticleNumber = "ING007", Name = "Salt" },
                new Ingredient { ArticleNumber = "ING008", Name = "Chocolate Chips" }
            };

            context.Ingredients.AddRange(ingredients);
            context.SaveChanges();

            var supplierIngredients = new List<SupplierIngredient>
            {
                new SupplierIngredient
                {
                    SupplierId = suppliers[0].SupplierId,
                    IngredientId = ingredients[0].IngredientId,
                    PricePerKg = 8.50m
                },
                new SupplierIngredient
                {
                    SupplierId = suppliers[0].SupplierId,
                    IngredientId = ingredients[5].IngredientId,
                    PricePerKg = 45.00m
                },
                new SupplierIngredient
                {
                    SupplierId = suppliers[1].SupplierId,
                    IngredientId = ingredients[1].IngredientId,
                    PricePerKg = 52.00m
                },
                new SupplierIngredient
                {
                    SupplierId = suppliers[1].SupplierId,
                    IngredientId = ingredients[3].IngredientId,
                    PricePerKg = 25.00m
                },
                new SupplierIngredient
                {
                    SupplierId = suppliers[2].SupplierId,
                    IngredientId = ingredients[2].IngredientId,
                    PricePerKg = 12.00m
                },
                new SupplierIngredient
                {
                    SupplierId = suppliers[2].SupplierId,
                    IngredientId = ingredients[4].IngredientId,
                    PricePerKg = 95.00m
                },
                new SupplierIngredient
                {
                    SupplierId = suppliers[2].SupplierId,
                    IngredientId = ingredients[7].IngredientId,
                    PricePerKg = 35.00m
                }
            };

            context.SupplierIngredients.AddRange(supplierIngredients);
            context.SaveChanges();

            var customers = new List<Customer>
            {
                new Customer
                {
                    StoreName = "Bella Bakery",
                    Phone = "+46761234567",
                    Email = "info@bellabakery.se",
                    ContactPerson = "Maria Sundström",
                    DeliveryAddress = "Bakvägen 5, 100 00 Stockholm",
                    InvoiceAddress = "Bakvägen 5, 100 00 Stockholm"
                },
                new Customer
                {
                    StoreName = "Golden Bread Café",
                    Phone = "+46762345678",
                    Email = "order@goldenbread.se",
                    ContactPerson = "Johan Berglund",
                    DeliveryAddress = "Cafévägen 10, 200 00 Gothenburg",
                    InvoiceAddress = "Cafévägen 10, 200 00 Gothenburg"
                },
                new Customer
                {
                    StoreName = "Sweet Dreams Pastry",
                    Phone = "+46763456789",
                    Email = "sales@sweetdreams.se",
                    ContactPerson = "Lisa Holm",
                    DeliveryAddress = "Pastellvägen 8, 300 00 Malmö",
                    InvoiceAddress = "Pastellvägen 8, 300 00 Malmö"
                }
            };

            context.Customers.AddRange(customers);
            context.SaveChanges();

            var products = new List<BakeryProduct>
            {
                new BakeryProduct
                {
                    ProductName = "Classic White Bread",
                    PricePerUnit = 45.00m,
                    Weight = 500m,
                    PackageQuantity = 1,
                    ExpirationDate = DateTime.UtcNow.AddDays(3),
                    ManufacturingDate = DateTime.UtcNow
                },
                new BakeryProduct
                {
                    ProductName = "Croissant",
                    PricePerUnit = 35.00m,
                    Weight = 80m,
                    PackageQuantity = 10,
                    ExpirationDate = DateTime.UtcNow.AddDays(1),
                    ManufacturingDate = DateTime.UtcNow
                },
                new BakeryProduct
                {
                    ProductName = "Chocolate Cake",
                    PricePerUnit = 180.00m,
                    Weight = 1000m,
                    PackageQuantity = 1,
                    ExpirationDate = DateTime.UtcNow.AddDays(2),
                    ManufacturingDate = DateTime.UtcNow
                },
                new BakeryProduct
                {
                    ProductName = "Vanilla Cupcake",
                    PricePerUnit = 50.00m,
                    Weight = 60m,
                    PackageQuantity = 12,
                    ExpirationDate = DateTime.UtcNow.AddDays(2),
                    ManufacturingDate = DateTime.UtcNow
                }
            };

            context.BakeryProducts.AddRange(products);
            context.SaveChanges();

            var orders = new List<Order>
            {
                new Order
                {
                    OrderNumber = "ORD001",
                    OrderDate = DateTime.UtcNow.AddDays(-5),
                    CustomerId = customers[0].CustomerId
                },
                new Order
                {
                    OrderNumber = "ORD002",
                    OrderDate = DateTime.UtcNow.AddDays(-2),
                    CustomerId = customers[1].CustomerId
                },
                new Order
                {
                    OrderNumber = "ORD003",
                    OrderDate = DateTime.UtcNow.AddDays(-1),
                    CustomerId = customers[2].CustomerId
                }
            };

            context.Orders.AddRange(orders);
            context.SaveChanges();

            var orderItems = new List<OrderItem>
            {
                new OrderItem
                {
                    OrderId = orders[0].OrderId,
                    BakeryProductId = products[0].BakeryProductId,
                    Quantity = 5,
                    UnitPrice = 45.00m,
                    TotalPrice = 225.00m
                },
                new OrderItem
                {
                    OrderId = orders[0].OrderId,
                    BakeryProductId = products[1].BakeryProductId,
                    Quantity = 2,
                    UnitPrice = 35.00m,
                    TotalPrice = 70.00m
                },
                new OrderItem
                {
                    OrderId = orders[1].OrderId,
                    BakeryProductId = products[2].BakeryProductId,
                    Quantity = 1,
                    UnitPrice = 180.00m,
                    TotalPrice = 180.00m
                },
                new OrderItem
                {
                    OrderId = orders[2].OrderId,
                    BakeryProductId = products[3].BakeryProductId,
                    Quantity = 3,
                    UnitPrice = 50.00m,
                    TotalPrice = 150.00m
                }
            };

            context.OrderItems.AddRange(orderItems);
            context.SaveChanges();
        }
    }
}