# Greener Admin

This repository hosts a Blazor-based reboot of the Greener Configurator together with the detailed implementation guide.
The solution follows the architecture outlined in `docs/ImplementationGuide.md` and already provides the first two feature
slices (locations and rules) via a hosted Blazor WebAssembly experience.

## Repository layout

```
.
├── docs/ImplementationGuide.md         # Full architecture / backlog description
└── src/
    ├── GreenerConfigurator.ClientCore  # Shared models + API services reused by UI projects
    ├── GreenerConfigurator.Web.Client  # Blazor WebAssembly SPA
    └── GreenerConfigurator.Web.Server  # ASP.NET Core host + lightweight API simulator
```

The server project references the client so it can serve the SPA directly (`UseBlazorFrameworkFiles`). It also exposes
`/api/1.0/location` and `/api/1.0/rule` endpoints backed by an in-memory repository to exercise the client services.

## Prerequisites

* .NET 8 SDK (`dotnet --version` should print 8.x)
* Node.js is **not** required; the UI is pure Blazor WebAssembly.

## Running the hosted experience

1. Restore and build the solution:
   ```bash
   dotnet build src/GreenerConfigurator.sln
   ```
2. Run the server host (it will automatically serve the WASM client and expose Swagger for the sample API):
   ```bash
   dotnet run --project src/GreenerConfigurator.Web.Server
   ```
3. Browse to the printed URL (typically `https://localhost:5001`). You can now:
   * Manage locations (CRUD) and nested details.
   * Review and edit rule schedules/metadata.
   * Read the implementation guide directly from the UI via the dashboard card.

The client consumes the shared `GreenerConfigurator.ClientCore` services, making it straightforward to plug in the
real Greener backend API endpoints as they become available.

## Configuring the real API and authentication

- The default API base address now targets the live endpoint at `https://api.greener.software/`. Override `Api:BaseAddress`
  in `src/GreenerConfigurator.Web.Client/wwwroot/appsettings.json` if you need to point at another environment.
- Azure AD B2C is wired up via MSAL. Replace the placeholders in the `AzureAdB2C` section (authority, client ID, scopes)
  with your tenant values so the SPA can acquire tokens and authorize calls against the protected API.
