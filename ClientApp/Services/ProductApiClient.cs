using System.Text.Json;
using ClientApp.Models;

namespace ClientApp.Services;

public sealed class ProductApiClient(HttpClient httpClient)
{
    private static readonly JsonSerializerOptions SerializerOptions = new(JsonSerializerDefaults.Web);
    private static readonly TimeSpan CacheDuration = TimeSpan.FromMinutes(1);

    private IReadOnlyList<Product>? cachedProducts;
    private DateTimeOffset cacheExpiresAt = DateTimeOffset.MinValue;

    public async Task<ProductQueryResult> GetProductsAsync(CancellationToken cancellationToken = default)
    {
        if (cachedProducts is not null && DateTimeOffset.UtcNow < cacheExpiresAt)
        {
            return ProductQueryResult.Success(cachedProducts, true);
        }

        try
        {
            using var response = await httpClient.GetAsync(
                "api/productlist",
                HttpCompletionOption.ResponseHeadersRead,
                cancellationToken);

            response.EnsureSuccessStatusCode();

            await using var responseStream = await response.Content.ReadAsStreamAsync(cancellationToken);
            var products = await JsonSerializer.DeserializeAsync<Product[]>(
                responseStream,
                SerializerOptions,
                cancellationToken);

            if (products is null)
            {
                return ProductQueryResult.Failure("The InventoryHub API returned an empty response.");
            }

            cachedProducts = products;
            cacheExpiresAt = DateTimeOffset.UtcNow.Add(CacheDuration);

            return ProductQueryResult.Success(products, false);
        }
        catch (TaskCanceledException) when (!cancellationToken.IsCancellationRequested)
        {
            return ProductQueryResult.Failure("The InventoryHub API request timed out.");
        }
        catch (HttpRequestException ex)
        {
            return ProductQueryResult.Failure($"The InventoryHub API request failed: {ex.Message}");
        }
        catch (JsonException ex)
        {
            return ProductQueryResult.Failure($"The InventoryHub API returned malformed JSON: {ex.Message}");
        }
    }
}
