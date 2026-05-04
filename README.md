Mormor Dagnys Bakery – REST API
Om projektet

Det här är ett REST API som jag byggt för att hantera ett bageri (Mormor Dagnys Bageri).

API:et hanterar:

leverantörer
råvaror (ingredienser)
kunder
produkter
beställningar

Det är ett rent API, vilket betyder:

inga webbsidor
inget frontend
inga views

Allt returneras som JSON och testas via Swagger eller direkt via URL.

Teknik

Projektet är byggt med:

ASP.NET Core Web API (.NET 9)
Entity Framework Core
SQLite (databas)
Swagger (för testning av endpoints)
Struktur
Controllers/ → API endpoints
Models/ → datamodeller
Data/ → DbContext + seed data
Program.cs → start och config
Hur man kör projektet
Öppna projektet i terminalen
Kör:
dotnet restore
dotnet build
dotnet run
Öppna i webbläsaren:
http://localhost:5000

Swagger startar direkt där och visar alla endpoints.

Databas
SQLite används
Fil: mormorsbageri.db
Skapas automatiskt vid första körning

Projektet använder seed data som läggs in om databasen är tom.

API – exempel på endpoints
Leverantörer
GET /api/suppliers
GET /api/suppliers/{id}
GET /api/suppliers/{id}/ingredients
GET /api/suppliers/search?name=...
POST /api/suppliers/{supplierId}/ingredients
PATCH /api/suppliers/{supplierId}/ingredients/{ingredientId}/price
Råvaror
GET /api/ingredients
GET /api/ingredients/search?name=...
Kunder
GET /api/customers
GET /api/customers/{id}
POST /api/customers
PATCH /api/customers/{id}/contact-person
Produkter
GET /api/bakeryproducts
GET /api/bakeryproducts/{id}
POST /api/bakeryproducts
PATCH /api/bakeryproducts/{id}/price
Beställningar
GET /api/orders
GET /api/orders/{id}
GET /api/orders/search?...
POST /api/orders
GET /api/orders/customers-by-products
