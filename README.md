# CryptoCurrency_BE

Backend of **Invest Smart**, a cryptocurrency trading simulator. It is an ASP.NET Core (.NET 8) solution that provides:

- a REST API for registration, login (JWT), the list of cryptocurrencies with their price history and the logged-in user's data,
- a SignalR hub for **buying and selling** cryptocurrencies in real time,
- a background service that **simulates price changes** and pushes every new price to clients over SignalR.

The Angular client lives in [CryptoCurrency_FE](https://github.com/AsyncApplicationsProject/CryptoCurrency_FE).

## Tech stack

- .NET 8, ASP.NET Core Web API
- ASP.NET Core SignalR
- Entity Framework Core 8 + SQL Server
- ASP.NET Core Identity + JWT Bearer authentication
- AutoMapper
- Swagger / Swashbuckle

## Solution structure

| Project | Description |
|---|---|
| `CryptoCurrency.WebAPI` | Main web application: REST controllers, `TradeHub` (`/tradeHub`), Identity + JWT configuration, Swagger. Seeds the database on startup. |
| `CryptoCurrency.WebHostedService` | Separate web application running `HostedService` (a `BackgroundService`) that generates new prices and broadcasts them through `PriceHistoryHub` (`/priceHistoryHub`). |
| `CryptoCurrency.WebHUB` | Definition of `PriceHistoryHub`. |
| `CryptoCurrency.Services` | Business logic: authentication, JWT, trading, users, price generation, AutoMapper profiles and DI registration (`Configuration/Dependencies.cs`). |
| `CryptoCurrency.DAL` | `AppDbContext` (EF Core + Identity), migrations and database seeding (`Seed/Seed.cs`). |
| `CryptoCurrency.Model` | Entities (`AppUser`, `Crypto`, `PriceHistory`, `UserCrypto`) and DTOs. |

## How it works

1. **WebAPI** starts and runs `Seed.Init`. It **clears the database** and fills it with sample data:
   - three cryptocurrencies: Bitcoin (`BTC`), Ethereum (`ETH`), Litecoin (`LTC`), each with 21 hourly prices,
   - three users with a random balance and a random wallet.
2. **WebHostedService** runs in parallel. Every 300–1500 ms it picks a random cryptocurrency, saves a new random price (20 000–60 000) to `PriceHistory` and sends it to all clients as the `PriceUpdate` event.
3. The client trades through `TradeHub`. A purchase or sale uses the latest price from the history, updates the user's balance and wallet in a database transaction and returns the result as the `TradeResponse` event.

> Prices are simulated. They do not come from a real exchange.

## REST API (`CryptoCurrency.WebAPI`)

| Method | Endpoint | Auth | Description |
|---|---|---|---|
| `POST` | `/api/auth/register` | – | Registers a user (`firstName`, `lastName`, `phoneNumber`, `email`, `password`). Returns `201 Created`, or `409 Conflict` if registration fails (e.g. the e-mail is already taken or the password does not meet Identity's rules). A new account gets a random starting balance and an empty wallet. |
| `POST` | `/api/auth/login` | – | Logs in with `email` and `password`. Returns the JWT as a string, or `401 Unauthorized`. |
| `GET` | `/api/crypto` | – | List of cryptocurrencies (`symbol`, `name`) with their full price history (`date`, `price`). |
| `GET` | `/api/user` | JWT | Logged-in user's data: `email`, `firstName`, `lastName`, `balance` and `wallet` (`cryptoSymbol`, `amount`). |

In the Development environment Swagger UI is available at `/swagger`, with support for the `Bearer` token.

## SignalR

### `TradeHub` – `/tradeHub` (WebAPI, requires JWT)

SignalR sends the token in the `access_token` query parameter.

| Client → server | Description |
|---|---|
| `Buy(symbol: string, amount: int)` | Buys `amount` units of the given cryptocurrency at its latest price. |
| `Sell(symbol: string, amount: int)` | Sells `amount` units (no more than the user owns). A wallet entry that drops to 0 is removed. |

| Server → client | Payload |
|---|---|
| `TradeResponse` | `{ isSuccess, message, userBalance, cryptoSymbol, cryptoAmount }` |

### `PriceHistoryHub` – `/priceHistoryHub` (WebHostedService, no authentication)

| Server → client | Payload |
|---|---|
| `PriceUpdate` | `{ symbol, name, date, price }`: the newest price of one cryptocurrency |

## Requirements

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- SQL Server (LocalDB is enough, e.g. the one installed with Visual Studio)
- EF Core tools for migrations: `dotnet tool install --global dotnet-ef`

## Configuration

### WebAPI: user secrets

`CryptoCurrency.WebAPI` reads its connection string and JWT settings from [user secrets](https://learn.microsoft.com/aspnet/core/security/app-secrets). From the solution folder, run:

```bash
cd CryptoCurrency.WebAPI
dotnet user-secrets set "ConnectionStrings:CryptoCurrencyDb" "Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=CryptoCurrencyDb;Integrated Security=True;Encrypt=False"
dotnet user-secrets set "JWT:Secret" "<a random key of at least 32 characters>"
dotnet user-secrets set "JWT:ValidIssuer" "https://localhost:7072"
dotnet user-secrets set "JWT:ValidAudience" "http://localhost:4200"
```

Optional: `JWTExtraSettings:TokenExpirySeconds`, the token lifetime in seconds (default `4500000`).

### WebHostedService: `appsettings.json`

The connection string `ConnectionStrings:CryptoCurrencyDb` is set in `CryptoCurrency.WebHostedService/appsettings.json` and points to LocalDB (`CryptoCurrencyDb`) by default. Both applications must use **the same database**.

### CORS

Both applications only accept requests from `http://localhost:4200`, the Angular dev server.

## Running

1. Create the database by applying the migrations:

   ```bash
   dotnet ef database update --project CryptoCurrency.DAL --startup-project CryptoCurrency.WebAPI
   ```

2. Start **both** applications with the `https` profile. The frontend connects to these exact addresses.

   ```bash
   dotnet run --project CryptoCurrency.WebAPI --launch-profile https
   ```

   ```bash
   dotnet run --project CryptoCurrency.WebHostedService --launch-profile https
   ```

   In Visual Studio you can instead set both projects as startup projects (*Configure Startup Projects → Multiple startup projects*).

| Application | HTTPS | HTTP |
|---|---|---|
| WebAPI (REST, `/tradeHub`, Swagger) | `https://localhost:7072` | `http://localhost:5222` |
| WebHostedService (`/priceHistoryHub`) | `https://localhost:7266` | `http://localhost:5266` |

3. Start the frontend ([CryptoCurrency_FE](https://github.com/AsyncApplicationsProject/CryptoCurrency_FE)) at `http://localhost:4200`.

## Test accounts

The seed creates these accounts, all with the password `Haslo123!`:

| E-mail | Name |
|---|---|
| `user1@example.com` | Mateusz Janik |
| `user2@example.com` | Kamil Czudaj |
| `user3@example.com` | Artur Buła |

> Every WebAPI start **deletes all data**, including accounts registered by hand, and generates new sample data.
