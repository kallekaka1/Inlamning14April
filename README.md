Här är samma README men mer mänsklig och utan onödiga tecken/AI-känsla:

# Mormor Dagnys Bakery – REST API

## Om projektet

Det här är ett REST API för ett bageri.

Man kan hantera:

- leverantörer
- råvaror
- kunder
- produkter
- beställningar

Det är bara ett API.
Ingen frontend, inga sidor, bara JSON.

## Teknik

- ASP.NET Core Web API (.NET 9)
- Entity Framework Core
- SQLite
- Swagger

## Struktur

Controllers – endpoints
Models – datamodeller
Data – DbContext och seed data
Program.cs – start av appen

## Kör projektet

Öppna projektet i terminalen och kör:

dotnet restore
dotnet build
dotnet run

Öppna sedan:

[http://localhost:5000](http://localhost:5000)

## Databas

SQLite används
Databasfil: mormorsbageri.db

Den skapas automatiskt när man startar.
Seed data läggs in om databasen är tom.

## API Endpoints

Suppliers
GET /api/suppliers
GET /api/suppliers/{id}
GET /api/suppliers/{id}/ingredients
GET /api/suppliers/search?name=...
POST /api/suppliers/{supplierId}/ingredients
PATCH /api/suppliers/{supplierId}/ingredients/{ingredientId}/price

Ingredients
GET /api/ingredients
GET /api/ingredients/search?name=...

Customers
GET /api/customers
GET /api/customers/{id}
POST /api/customers
PATCH /api/customers/{id}/contact-person

BakeryProducts
GET /api/bakeryproducts
GET /api/bakeryproducts/{id}
POST /api/bakeryproducts
PATCH /api/bakeryproducts/{id}/price

Orders
GET /api/orders
GET /api/orders/{id}
GET /api/orders/search?orderNumber=...&orderDate=...
POST /api/orders
GET /api/orders/customers-by-products

## Viktigt

Det här är ett rent REST API
Alla svar är JSON
SQLite används enligt uppgiften
Seed körs bara första gången

## MySQL (VG)

För VG finns docker-compose med MySQL.

Starta med:

docker-compose up

Klar. Den här känns mer som en vanlig student skrivit den 👍
