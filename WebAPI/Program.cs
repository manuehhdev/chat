using ApplicationCore.Entities;
using Infrastructure.Data;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddOpenApi();

//Add data context
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration
        .GetConnectionString("DefaultConnection"), 
        x => x.UseNetTopologySuite()));

//Add user context
builder.Services.AddDbContext<UsersDbContext>(options => 
    options.UseSqlServer(builder.Configuration
        .GetConnectionString("UsersConnection")));
builder.Services.AddDataProtection();

builder.Services.AddIdentityCore<Usuario>()
    .AddEntityFrameworkStores<UsersDbContext>()
    .AddDefaultTokenProviders();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.Run();