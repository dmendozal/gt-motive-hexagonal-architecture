# Repository Guidelines

## Project Structure & Module Organization

This repository contains a .NET microservice solution. The main solution file is `src/microservice.sln`.

- `src/GtMotive.Estimate.Microservice.Domain`: domain model and core business concepts.
- `src/GtMotive.Estimate.Microservice.ApplicationCore`: application services and use-case orchestration.
- `src/GtMotive.Estimate.Microservice.Api`: API-layer filters, presenters, authorization, and dependency injection.
- `src/GtMotive.Estimate.Microservice.Infrastructure`: persistence and external integrations, including MongoDB.
- `src/GtMotive.Estimate.Microservice.Host`: ASP.NET host, configuration, telemetry, Dockerfile, and app settings.
- `test/unit`, `test/infrastructure`, `test/functional`: xUnit test projects grouped by test scope.

Shared build and package settings live in `Directory.Build.props`, `Directory.Build.targets`, and `test/Directory.Build.props`.

## Build, Test, and Development Commands

Run commands from `src/` unless noted otherwise.

- `dotnet restore`: restores all solution packages and runs NuGet vulnerability audit.
- `dotnet build microservice.sln --no-restore`: builds the full solution after restore.
- `dotnet build GtMotive.Estimate.Microservice.Host/GtMotive.Estimate.Microservice.Host.csproj --no-restore -m:1`: builds the host and dependencies with reduced parallelism.
- `dotnet test ../test/unit/GtMotive.Estimate.Microservice.UnitTests/GtMotive.Estimate.Microservice.UnitTests.csproj`: runs unit tests.
- `dotnet test ../test/infrastructure/GtMotive.Estimate.Microservice.InfrastructureTests/GtMotive.Estimate.Microservice.InfrastructureTests.csproj`: runs infrastructure tests.
- `dotnet test ../test/functional/GtMotive.Estimate.Microservice.FunctionalTests/GtMotive.Estimate.Microservice.FunctionalTests.csproj`: runs functional tests.

## Coding Style & Naming Conventions

The repo enables `AnalysisMode=AllEnabledByDefault`, `EnforceCodeStyleInBuild=true`, and `TreatWarningsAsErrors=true`. StyleCop and SonarAnalyzer are part of every project, so keep builds warning-free.

Use four-space indentation for C# and XML project files. Follow .NET naming conventions: PascalCase for public types and members, camelCase for locals and parameters, and interfaces prefixed with `I`.

## Implementation Conventions

Follow the architecture guidance in `README.md` for every implementation. Use cases are the central organizing structure and must stay decoupled from frameworks, persistence, and delivery details.

- Keep business rules inside the hexagon: `Domain` and `ApplicationCore` must not depend on web, database, cloud, or infrastructure frameworks.
- Model application boundaries with ports. Primary actors drive the application through request/use-case ports; secondary actors are reached through repository, unit-of-work, or service ports.
- Implement adapters outside the core. Controllers, presenters, persistence, telemetry, and external clients belong in `Api`, `Infrastructure`, or `Host`.
- Use MVC controllers for HTTP endpoints, matching the base solution design.
- Use MediatR for command/query entry points where useful. Controllers should translate HTTP requests into application requests and delegate business work to handlers or use cases.
- Use `DomainException` for expected validation and business-rule failures in the domain and application core. The API already registers `BusinessExceptionFilter` to translate those exceptions into HTTP responses.
- Handlers should build input models and call use cases. Do not build HTTP responses in handlers; use cases write to output ports.
- Use presenters as output adapters for successful responses. Let `DomainException` bubble to the API exception filter for expected failures.
- Treat view models as framework-facing DTOs. Add XML comments and `[Required]` where needed for Swagger and nullable clarity.
- Apply DDD patterns deliberately: value objects for validated concepts, entities for identity and lifecycle, aggregate roots for consistency boundaries, repositories for persistence abstraction, and domain services only for business behavior that does not belong to one entity. Organize domain code by business context first, then tactical DDD role, for example `Domain/Vehicles/AggregateRoots`, `Domain/Vehicles/ValueObjects`, `Domain/Vehicles/Enums`, `Domain/Rentals/AggregateRoots`, and `Domain/People/Entities`.
- Keep reusable business error messages under `Domain/Common/Errors` and throw `DomainException` with those messages when a rule is violated.
- When persisting to MongoDB, enforce business uniqueness with indexes where possible, especially `Vehicle.Vin` and one active rental per `PersonDocumentId`.
- Use `IUnitOfWork` for use cases that mutate multiple aggregates. MongoDB transactions require Mongo to run as a replica set, including local Docker and Testcontainers.
- Treat infrastructure selection as an explicit configuration value. Use `Infrastructure:Provider` with closed values such as `InMemory` and `Mongo`; do not fall back silently from environment detection.
- Organize `ApplicationCore` by feature and use case. Keep application ports in `ApplicationCore/Ports`, and group use-case artifacts under feature folders, for example `ApplicationCore/Vehicles/CreateVehicle/CreateVehicleUseCase.cs`, `CreateVehicleInput.cs`, `CreateVehicleOutput.cs`, and `ICreateVehicleOutputPort.cs`.
- Organize `Api` by feature and use case as well. Prefer small controllers per use case when Sonar responsibilities would be mixed, for example `Api/Vehicles/CreateVehicle/CreateVehicleController.cs` and `Api/Vehicles/GetAvailableVehicles/GetAvailableVehiclesController.cs`. Place request, response, and presenter models for each use case in the same use-case folder.
- Keep physical folders and namespaces aligned. Do not leave types in generic namespaces after moving them into feature-specific folders.

## Testing Guidelines

Tests use xUnit, FluentAssertions, and coverlet. Place new tests in the matching scope under `test/`. Name test classes after the subject under test and use descriptive method names that state the expected behavior.

Prefer focused unit tests for domain/application logic. Use infrastructure or functional tests only when integration wiring, persistence, or host behavior is part of the behavior being verified.
Functional tests should exercise the real HTTP host against MongoDB through Testcontainers when persistence is part of the scenario. Reset state between scenarios so each test remains independent.

## Commit & Pull Request Guidelines

The current history uses short imperative messages such as `resolve dependencies`. Keep commits concise and action-oriented, for example `fix nuget audit warnings` or `add vehicle estimate tests`.

Pull requests should include a brief summary, the reason for the change, affected projects, and verification commands run. Link related issues when available. For dependency changes, call out security advisories, package versions, and any audit suppressions.

## Security & Configuration Tips

Do not commit secrets in `appsettings*.json`. Use user secrets, environment variables, or deployment configuration for credentials. Keep NuGet audit warnings visible; suppress advisories only when no fixed package is available and document the reason.
