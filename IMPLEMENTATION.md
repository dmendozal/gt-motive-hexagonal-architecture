# Implementation Overview

## Scope

This repository implements a fleet rental microservice for a renting company. The current solution covers:

- create a vehicle
- list available vehicles
- rent a vehicle
- return a rental

The source of truth for business rules is `docs/specs/fleet-rental.md`.

## Architecture

The solution follows hexagonal architecture with DDD-style organization:

- `Domain`: entities, value objects, aggregate roots, and domain exceptions.
- `ApplicationCore`: use cases and ports.
- `Api`: MVC controllers, request/response models, and presenters.
- `Infrastructure`: MongoDB and in-memory adapters, time provider, logging, telemetry.
- `Host`: composition root, middleware, configuration, and Swagger.

Controllers call use cases directly. Use cases write success responses through presenters. Expected business failures are represented with `DomainException` and translated to HTTP by the API filter.

## Domain Model

- `Vehicle` is the main fleet aggregate.
- `Rental` models an active or completed rental.
- `Person` is represented by a domain entity and its `DocumentId` value object.
- `Vin`, `VehicleId`, `DocumentId`, and `PersonId` are value objects.

Mongo persistence uses separate `PersistenceModel` types in `Infrastructure` and maps them to domain aggregates.

## Persistence

The application supports in-memory and MongoDB repositories. The host selects the provider explicitly through `Infrastructure:Provider`; it does not infer persistence from the environment name. Functional tests run against real MongoDB through Testcontainers. Mongo repositories enforce the main uniqueness rules with indexes:

- unique `vin` for vehicles
- one active rental per `personDocumentId`

`RentVehicle` and `ReturnRental` execute their multi-aggregate changes through `IUnitOfWork`. The Mongo adapter uses client sessions and transactions, so local Docker and functional tests run MongoDB as a single-node replica set.

## Testing

- `unit`: domain and application behavior.
- `infrastructure`: host wiring and endpoint validation.
- `functional`: real HTTP host plus MongoDB through Testcontainers.

## Run

From `src/`:

```bash
dotnet restore
dotnet test ../test/unit/GtMotive.Estimate.Microservice.UnitTests/GtMotive.Estimate.Microservice.UnitTests.csproj --no-restore
dotnet test ../test/infrastructure/GtMotive.Estimate.Microservice.InfrastructureTests/GtMotive.Estimate.Microservice.InfrastructureTests.csproj --no-restore
dotnet test ../test/functional/GtMotive.Estimate.Microservice.FunctionalTests/GtMotive.Estimate.Microservice.FunctionalTests.csproj --no-restore
dotnet build GtMotive.Estimate.Microservice.Host/GtMotive.Estimate.Microservice.Host.csproj --no-restore -m:1
```

Docker must be available locally for `functional` tests because Testcontainers starts MongoDB containers on demand.
