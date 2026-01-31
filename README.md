# Famous Quote Quiz

A simple quiz application where users guess the authors of famous quotes.

## Prerequisites

- .NET 8.0 SDK or later
- SQL Server

## How to Run

1. **Clone the repository**
   ```bash
   git clone <repository-url>
   cd FamousQuoteQuiz
   ```

2. **Update the connection string** (optional)
   
   Edit `FamousQuoteQuiz.Web/appsettings.json` if needed. Default uses LocalDB.

3. **Run the application**
   ```bash
   dotnet run --project FamousQuoteQuiz.Web
   ```

   The database will be created and seeded automatically on first run.

4. **Open in browser**

## Default Login Credentials

### Guest User
- **Email:** guest@email
- **Password:** password

### Admin User
- **Email:** admin@email
- **Password:** admin

Admins have access to user management, quote management, and user achievements.

## Technologies

- ASP.NET Core MVC
- Clean Architecture
- MediatR (CQRS pattern)
- Entity Framework Core
- SQL Server
- Bootstrap 5

## Project Structure

- `FamousQuoteQuiz.Web` - Presentation layer (MVC)
- `FamousQuoteQuiz.Application` - Business logic (CQRS commands/queries)
- `FamousQuoteQuiz.Domain` - Domain entities and interfaces
- `FamousQuoteQuiz.Infrastructure` - Data access and external services

## License

See LICENSE file for details.
