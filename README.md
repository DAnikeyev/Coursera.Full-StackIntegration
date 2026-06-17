# InventoryHub

InventoryHub is a full-stack sample application built for the Coursera full-stack integration assignment. It combines a **Blazor WebAssembly** front end with an **ASP.NET Core Minimal API** back end to demonstrate front-end/back-end communication, integration debugging, structured JSON responses, and performance optimization.

## Repository contents

- `ClientApp` - Blazor WebAssembly front end
- `ServerApp` - Minimal API back end
- `FullStackSolution.slnx` - solution file for the combined project
- `REFLECTION.md` - assignment reflection describing how Copilot helped during development

## InventoryHub features

- Fetches products from the back-end route at `/api/productlist`
- Displays product name, category, price, and stock in the Blazor UI
- Handles API failures, timeouts, and malformed JSON safely in the client
- Returns standardized nested JSON from the API
- Uses client-side request caching and server-side output caching to reduce redundant work
- Enables CORS so the front end can access the back end during local development

## Running the solution

1. Start the back end:
   `dotnet run --project .\ServerApp\ServerApp.csproj`
2. Start the front end in a second terminal:
   `dotnet run --project .\ClientApp\ClientApp.csproj`
3. Open `http://localhost:5158` and browse to `/fetchproducts`.
4. The API is available at `http://localhost:5155/api/productlist`.
