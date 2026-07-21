# TaskFlow - Clean Architecture Project Management API

[![Build Status](https://img.shields.io/badge/build-passing-brightgreen)]()
[![Tests](https://img.shields.io/badge/tests-32%20passing-brightgreen)]()
[![.NET Version](https://img.shields.io/badge/.NET-10.0-blue)]()

A production-ready mini-JIRA API built with Clean Architecture, demonstrating modern design patterns and enterprise best practices.

## 🎯 What is TaskFlow?

TaskFlow enables collaborative project management with:
- 🏢 **Organizations** - Multi-tenant workspaces
- 📊 **Projects** - Organized work containers
- ✅ **Tasks** - Individual work items with assignments
- 👥 **Role-Based Access** - Admin/Member permissions
- 💬 **Collaboration** - Comments and reactions

## 🚀 Quick Start

### Prerequisites
- .NET 10 SDK
- SQL Server (LocalDB or Express)

### Setup
```bash
# Clone and restore
git clone https://github.com/niframaz/taskflow-api
cd taskflow-api
dotnet restore

# Update connection string in appsettings.Development.json

# Run migrations
dotnet ef database update --project TaskFlow.Infrastructure --startup-project TaskFlow.Api

# Run API
dotnet run --project TaskFlow.Api
# → https://localhost:7059
# → https://localhost:7059/scalar/v1 (API docs)
```

### Run Tests
```bash
dotnet test
# Expected: 32/32 passing ✅
```

## 📚 Key Endpoints

### Authentication
```http
POST /api/users/register   # Register new user
POST /api/users/login       # Login (returns JWT)
```

### Organizations
```http
GET    /api/organizations           # List my organizations
GET    /api/organizations/{id}      # Get details
POST   /api/organizations           # Create (auto-admin)
PUT    /api/organizations/{id}      # Update (admin only)
DELETE /api/organizations/{id}      # Delete (admin only)
```

### Projects & Tasks
```http
GET  /api/projects                    # List my projects
POST /api/projects                    # Create (admin only)
GET  /api/taskitems/project/{id}      # List project tasks
POST /api/taskitems                   # Create task
POST /api/taskitems/{id}/assign       # Assign to user
```

## 🏗️ Architecture

### Clean Architecture Layers
```
┌─────────────────────────────────┐
│   API Layer                      │  Controllers, Middleware
├─────────────────────────────────┤
│   Application Layer              │  Services, Abstractions, DTOs
├─────────────────────────────────┤
│   Domain Layer                   │  Entities, Enums
├─────────────────────────────────┤
│   Infrastructure Layer           │  EF Core, Repositories, Identity
└─────────────────────────────────┘
```

### Design Patterns

- ✅ **Repository Pattern** - Data access abstraction
- ✅ **Unit of Work** - Transaction management  
- ✅ **Result Pattern** - Error handling without exceptions
- ✅ **DTO Pattern** - API/Domain separation (DTOs in Application layer)
- ✅ **Dependency Injection** - Constructor injection throughout

### Architectural Decisions

**Pragmatic Approach**: This project balances Clean Architecture principles with practical simplicity.

**Anemic Domain Models**
- Entities are data-focused (properties only)
- Business logic lives in Application services
- ✅ Simpler for CRUD-heavy operations
- ✅ Easier onboarding for team members
- ⚠️ Trade-off: Not pure DDD with rich models

**Service Responsibilities**  
- Services handle authorization, validation, orchestration
- ✅ Clear transaction boundaries
- ✅ Single entry point per use case
- ⚠️ Could split further for strict SRP

**Why This Works**: For CRUD applications at this scale, service-centric architecture provides clarity without DDD complexity. For larger domains, consider rich models, CQRS, and domain events.

## 🔒 Security

- **JWT Authentication** - Secure token-based auth
- **Role-Based Authorization** - Admin/Member per organization
- **Rate Limiting** - 60 req/min, 5 login attempts/5min  
- **Input Validation** - Comprehensive DTO validation
- **SQL Injection Protection** - EF Core parameterized queries
- **Optimistic Concurrency** - Timestamp-based conflict detection

## 🧪 Testing

### Test Coverage (32 Passing)
- **OrganizationService** - Business logic tests
- **OrganizationRepository** - Data access tests
- **OrganizationsController** - API endpoint tests
- **Integration Tests** - End-to-end scenarios

```bash
# Unit tests only
dotnet test --filter "FullyQualifiedName!~Integration"

# All tests
dotnet test --verbosity normal
```

## 📦 Technology Stack

### Core
- **.NET 10** - Latest framework
- **ASP.NET Core Identity** - Authentication
- **Entity Framework Core 10** - ORM
- **SQL Server** - Database

### Libraries
- **AutoMapper** - Object mapping
- **Serilog** - Structured logging
- **AspNetCoreRateLimit** - Rate limiting
- **xUnit + Moq + FluentAssertions** - Testing

## ⚙️ Configuration

### Required Settings
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=TaskFlow;Trusted_Connection=true"
  },
  "JwtSettings": {
    "Secret": "your-secret-key-min-32-chars",
    "ExpirationInMinutes": 600
  }
}
```

### Environment Files
- `appsettings.Development.json` - Development config
- `appsettings.Production.json` - Production config

## 📖 API Documentation

- **Scalar UI** - Interactive API docs at `https://localhost:7059/scalar/v1`
- **OpenAPI/Swagger** - Standard OpenAPI 3.0 specification available

## 🚦 Production Checklist

Before deployment:
- [ ] Move secrets to Azure Key Vault/environment variables
- [ ] Update CORS origins for production
- [ ] Enable HTTPS enforcement (HSTS)
- [ ] Configure production logging (Application Insights)
- [ ] Review rate limits
- [ ] Set up database backups
- [ ] Add health check monitoring

## 🎯 Project Highlights

### Enterprise Patterns
✨ **Clean Architecture** - Proper layer separation with dependency inversion  
✨ **Result Pattern** - Functional error handling  
✨ **Repository + UoW** - Data access abstraction with transactions  
✨ **DTO Pattern** - DTOs in Application layer (correct Clean Architecture)  
✨ **SOLID Principles** - Single Responsibility, Dependency Inversion

### Production Features
- Comprehensive input validation
- Optimistic concurrency control
- Database indexes for performance
- Memory caching for user/membership data
- Structured logging with Serilog
- Rate limiting for security
- Health checks

### Code Quality
- 32 unit tests (100% pass rate)
- Integration tests with in-memory database
- Zero build warnings (code-related)
- Clean code - removed unnecessary comments

## 📊 Statistics

- **Lines of Code**: ~5,000+
- **Test Coverage**: 32 tests (all passing)
- **API Endpoints**: 25+
- **Build Status**: ✅ Passing
- **Architectural Layers**: 4 (API, Application, Domain, Infrastructure)

## 🔮 Future Enhancements

Documented for learning/expansion:
- Implement pagination for large datasets
- Add comprehensive integration tests
- Rich domain models with invariants (DDD)
- CQRS for complex read/write scenarios
- Domain events for loose coupling
- API versioning
- SignalR for real-time updates

## 📝 License

MIT License

## 👤 Author

**Mohamed Nifras**
- GitHub: [@niframaz](https://github.com/niframaz)

---

⭐ **Portfolio Project** - Demonstrates Clean Architecture, modern .NET patterns, and production-ready API development

*Built to showcase enterprise-level software engineering skills*
