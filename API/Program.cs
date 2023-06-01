using API.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddDbContext<DataContext>(opt =>
{
    opt.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"));
}
);
builder.Services.AddCors();

var app = builder.Build();

// Configure the HTTP request pipeline. This is also known as the middleware

app.UseCors(builder =>

    builder.AllowAnyHeader().AllowAnyMethod().WithOrigins("https://localhost:4200"));

//app.UseAuthorization();

app.MapControllers();//  this is the middleware that will tell our request which API Endpoint needs to go to


app.Run();
