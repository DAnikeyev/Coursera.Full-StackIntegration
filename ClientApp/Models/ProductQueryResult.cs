namespace ClientApp.Models;

public sealed record ProductQueryResult(
    IReadOnlyList<Product> Products,
    string? ErrorMessage,
    bool IsFromCache)
{
    public bool HasError => !string.IsNullOrWhiteSpace(ErrorMessage);

    public static ProductQueryResult Success(IReadOnlyList<Product> products, bool isFromCache) =>
        new(products, null, isFromCache);

    public static ProductQueryResult Failure(string message) =>
        new(Array.Empty<Product>(), message, false);
}
