# Enterprise BPM System

A comprehensive, production-ready Business Process Management (BPM) platform built with .NET 8 and modern architecture. This system provides BPMN 2.0 compliant workflow execution, dynamic forms, business rules engine, and extensive integration capabilities.

## 🎯 Project Overview

This Enterprise BPM System is a complete solution for managing business processes, similar to K2 but with modern architecture and enhanced features. It includes:

- **Workflow Engine**: BPMN 2.0 compliant process execution
- **Forms Engine**: Dynamic form builder and renderer
- **Rules Engine**: DMN compliant decision management
- **Integration Hub**: Connectors for REST, SOAP, databases, email, and more
- **Analytics**: Real-time dashboards and process mining
- **Security**: JWT authentication, OAuth 2.0, Azure AD integration
- **Multi-tenancy**: Complete tenant isolation and management

## 🏗️ Architecture

The solution follows Clean Architecture principles with clear separation of concerns:

```
EnterpriseBPM/
├── src/
│   ├── Core/                       # Business logic and domain models
│   │   ├── BPM.Core.Domain/       # Entities, interfaces, enums
│   │   ├── BPM.Core.WorkflowEngine/  # BPMN workflow execution
│   │   ├── BPM.Core.RulesEngine/  # DMN decision engine
│   │   └── BPM.Core.FormsEngine/  # Dynamic forms
│   ├── Services/                   # REST APIs and microservices
│   │   ├── BPM.Services.Process/  # Process management APIs
│   │   ├── BPM.Services.Forms/    # Forms APIs
│   │   ├── BPM.Services.Identity/ # Authentication APIs
│   │   ├── BPM.Services.Integration/  # Integration APIs
│   │   └── BPM.Services.Analytics/  # Analytics & reporting
│   ├── Infrastructure/             # Cross-cutting concerns
│   │   ├── BPM.Infrastructure.Data/  # EF Core, repositories
│   │   ├── BPM.Infrastructure.Messaging/  # RabbitMQ, Azure Service Bus
│   │   ├── BPM.Infrastructure.Cache/  # Redis caching
│   │   ├── BPM.Infrastructure.Storage/  # File storage
│   │   └── BPM.Infrastructure.Logging/  # Serilog
│   ├── Designer/                   # Visual designers (React)
│   └── Portal/                     # User portals
└── tests/                          # Unit and integration tests
```

## 🚀 Getting Started

### Prerequisites

- .NET 8.0 SDK or later
- SQL Server 2019+ or PostgreSQL 13+
- Node.js 18+ (for React designers)
- Docker and Docker Compose (optional, for containerized deployment)
- Redis (optional, for caching)
- RabbitMQ (optional, for messaging)

### Installation

1. **Clone the repository**
   ```bash
   git clone https://github.com/AnwarFaiez/BPM_Project.git
   cd BPM_Project
   ```

2. **Build the solution**
   ```bash
   dotnet restore
   dotnet build
   ```

3. **Configure database connection**
   
   Update the connection string in `appsettings.json` for each service:
   ```json
   {
     "ConnectionStrings": {
       "DefaultConnection": "Server=localhost;Database=EnterpriseBPM;Trusted_Connection=True;TrustServerCertificate=True"
     }
   }
   ```

4. **Run database migrations**
   ```bash
   cd src/Infrastructure/BPM.Infrastructure.Data
   dotnet ef migrations add InitialCreate
   dotnet ef database update
   ```

5. **Run services**
   ```bash
   # Run Process Service
   cd src/Services/BPM.Services.Process
   dotnet run

   # Run Identity Service
   cd src/Services/BPM.Services.Identity
   dotnet run

   # Run Forms Service
   cd src/Services/BPM.Services.Forms
   dotnet run
   ```

### Docker Deployment

```bash
# Build and run all services
docker-compose up -d

# View logs
docker-compose logs -f

# Stop all services
docker-compose down
```

## 📚 Documentation

- [Architecture Guide](docs/architecture/README.md)
- [API Documentation](docs/api/README.md)
- [User Guide](docs/user-guide/README.md)
- [Deployment Guide](docs/deployment/README.md)

## 🎨 Core Features

### Workflow Engine (BPMN 2.0)
- Start events, end events, intermediate events
- User tasks, service tasks, script tasks
- Exclusive, parallel, and inclusive gateways
- Process versioning and dynamic modification
- Error handling and compensation

### Forms Engine
- Drag-and-drop form designer
- Rich control library (text, dropdown, file upload, signature, etc.)
- Conditional visibility and validation
- Calculated fields and cascading dropdowns
- Mobile-friendly responsive design

### Rules Engine (DMN)
- Decision tables and decision trees
- C# expression language
- Rule versioning and testing
- Business glossary

### Integration Hub
- REST and SOAP connectors
- Database connectors (SQL Server, PostgreSQL, MySQL, Oracle)
- Email (SMTP, Exchange, Office 365)
- SMS, file systems, cloud storage
- Custom connector SDK

### Security
- JWT token authentication
- OAuth 2.0 / OpenID Connect
- Azure AD integration
- Role-based access control (RBAC)
- Multi-factor authentication

## 🛠️ Technology Stack

### Backend
- .NET 8 (ASP.NET Core Web API)
- Entity Framework Core 8
- Dapper (performance-critical queries)
- MediatR (CQRS pattern)
- FluentValidation
- AutoMapper
- Hangfire (background jobs)
- SignalR (real-time communication)
- Serilog (structured logging)

### Frontend
- React 18+ with TypeScript
- bpmn-js (workflow designer)
- React Flow
- Ant Design / Material-UI
- Redux Toolkit
- React Query

### Database
- SQL Server (primary)
- PostgreSQL (alternative)
- Redis (caching)
- Elasticsearch (search & analytics)

### DevOps
- Docker & Docker Compose
- Kubernetes
- GitHub Actions (CI/CD)

## 📊 API Endpoints

### Process Service (Port 5001)
- `POST /api/processes` - Create process definition
- `GET /api/processes/{id}` - Get process definition
- `POST /api/processes/{id}/start` - Start workflow instance
- `GET /api/workflows/{id}` - Get workflow instance
- `POST /api/tasks/{id}/complete` - Complete task

### Identity Service (Port 5002)
- `POST /api/auth/login` - User login
- `POST /api/auth/register` - User registration
- `GET /api/users` - List users
- `POST /api/roles` - Create role

### Forms Service (Port 5003)
- `POST /api/forms` - Create form definition
- `GET /api/forms/{id}` - Get form definition
- `POST /api/forms/{id}/submit` - Submit form data

## 🧪 Testing

### Run Unit Tests
```bash
dotnet test tests/BPM.Tests.Unit
```

### Run Integration Tests
```bash
dotnet test tests/BPM.Tests.Integration
```

### Run All Tests
```bash
dotnet test
```

## 🤝 Contributing

Contributions are welcome! Please read the [Contributing Guide](CONTRIBUTING.md) for details on our code of conduct and the process for submitting pull requests.

## 📝 License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## 🔗 Links

- [Project Website](https://github.com/AnwarFaiez/BPM_Project)
- [Documentation](docs/)
- [Issue Tracker](https://github.com/AnwarFaiez/BPM_Project/issues)

## 📧 Contact

For questions or support, please open an issue on GitHub or contact the maintainers.

---

**Status**: 🚧 Under Active Development

This is a comprehensive enterprise BPM platform. Phase 1 (Foundation) is complete with core domain models, infrastructure, and basic workflow engine implementation.