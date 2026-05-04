using Microsoft.EntityFrameworkCore;
using MorMorsBageruMVC.Data;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "Mormor Dagnys Bakery API",
        Version = "v1",
        Description = "REST API for managing suppliers, ingredients, customers, bakery products, and orders"
    });
});


var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? "Data Source=mormorsbageri.db";

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(connectionString));

var app = builder.Build();


using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    DbInitializer.Initialize(context);
}


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Mormor Dagnys API v1");
        c.RoutePrefix = string.Empty; // Serve Swagger UI at root
    });
}

app.UseHttpsRedirection();
app.UseAuthorization();

app.MapControllers();

app.Run();

