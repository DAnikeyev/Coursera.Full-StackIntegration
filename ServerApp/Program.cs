using System.Text.Json;
using Microsoft.AspNetCore.OutputCaching;
using ServerApp.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.Services.AddCors();
builder.Services.AddOutputCache();
builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
});

var products = new[]
{
    new ProductResponse(
        1,
        "Laptop",
        1200.50m,
        25,
        new CategoryResponse(101, "Electronics")),
    new ProductResponse(
        2,
        "Headphones",
        50.00m,
        100,
        new CategoryResponse(102, "Accessories"))
};

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseCors(policy =>
    policy.AllowAnyOrigin()
          .AllowAnyMethod()
          .AllowAnyHeader());

app.UseOutputCache();
app.UseHttpsRedirection();

app.MapGet("/api/productlist", () => TypedResults.Ok(products))
    .WithName("GetProductList")
    .CacheOutput(policy => policy.Expire(TimeSpan.FromMinutes(5)));

app.Run();
