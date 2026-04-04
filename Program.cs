using Microsoft.EntityFrameworkCore;
using MorMorsBageruMVC.Data;
using MorMorsBageruMVC.Models;

var builder = WebApplication.CreateBuilder(args);

// Services
builder.Services.AddControllers();
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite("Data Source=mormorsbageri.db"));

var app = builder.Build();

// Skapa databasen om den inte finns
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

    db.Database.EnsureCreated();

    if (!db.Leverantörer.Any())
    {
        var lev1 = new Leverantör
        {
            Namn = "ICA",
            Adress = "Stockholm",
            Kontaktperson = "Anna",
            Telefon = "0701234567",
            Epost = "ica@mail.se"
        };

        var lev2 = new Leverantör
        {
            Namn = "Coop",
            Adress = "Göteborg",
            Kontaktperson = "Erik",
            Telefon = "0707654321",
            Epost = "coop@mail.se"
        };

        var rav1 = new Råvara
        {
            Artikelnummer = "R001",
            Namn = "Socker",
            PrisPerKg = 12
        };

        var rav2 = new Råvara
        {
            Artikelnummer = "R002",
            Namn = "Mjöl",
            PrisPerKg = 8
        };

        db.Leverantörer.AddRange(lev1, lev2);
        db.Råvaror.AddRange(rav1, rav2);
        db.SaveChanges();

        db.LeverantörRåvaror.AddRange(
            new LeverantörRåvara
            {
                LeverantörId = lev1.LeverantörId,
                RåvaraId = rav1.RåvaraId,
                Pris = 12
            },
            new LeverantörRåvara
            {
                LeverantörId = lev2.LeverantörId,
                RåvaraId = rav1.RåvaraId,
                Pris = 10
            },
            new LeverantörRåvara
            {
                LeverantörId = lev1.LeverantörId,
                RåvaraId = rav2.RåvaraId,
                Pris = 8
            }
        );

        db.SaveChanges();
    }
}

// Pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthorization();

app.MapControllers();

app.Run();