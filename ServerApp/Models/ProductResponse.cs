namespace ServerApp.Models;

public sealed record ProductResponse(
    int Id,
    string Name,
    decimal Price,
    int Stock,
    CategoryResponse Category);
