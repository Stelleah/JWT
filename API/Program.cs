using System.Text;
using API.Data;
using API.Extensions;
using API.Interfaces;
using API.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddApplicationServices(builder.Configuration);
builder.Services.AddIdentityServices(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline. This is also known as the middleware

app.UseCors(builder =>

    builder.AllowAnyHeader().AllowAnyMethod().WithOrigins("https://localhost:4200"));
    
//adding two pieces of middleware
app.UseAuthentication();// asks for the token
app.UseAuthorization();// what are you allowed to do?

app.MapControllers();//  this is the middleware that will tell our request which API Endpoint needs to go to


app.Run();
