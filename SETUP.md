# Running the Microservice

This guide explains how to run the solution locally and from the IDE.

## Prerequisites

- .NET 9 SDK
- Docker Desktop or Docker Engine
- MongoDB Compass if you want to inspect the database

## Project Modes

The host uses an explicit infrastructure provider:

- `Infrastructure:Provider = InMemory` for local development and unit-style runs
- `Infrastructure:Provider = Mongo` for Docker and end-to-end runs

## Run with Docker

The repository includes a Docker Compose setup that starts:

- `host` on `http://localhost:8080`
- `mongodb` on `localhost:27017`

MongoDB runs as a single-node replica set because the application uses Mongo transactions for multi-aggregate operations.

Steps:

1. Start Docker Desktop.
2. Open the solution in Visual Studio or Rider.
3. Use the Docker Compose run configuration.
4. Run the `host` service.

If you prefer the CLI:

```bash
docker compose up -d
```

The host reads these settings from `docker-compose.override.yml`:

- `ASPNETCORE_ENVIRONMENT=Development`
- `Infrastructure__Provider=Mongo`
- `MongoDb__ConnectionString=mongodb://mongodb:27017/?replicaSet=rs0`
- `MongoDb__MongoDbDatabaseName=estimate`

## Run from Visual Studio

1. Open `src/microservice.sln`.
2. Set the startup project to **Docker Compose**.
3. Press `F5` or `Run`.
4. Visual Studio will start MongoDB and the API together.

If Docker Compose does not appear as a startup option, reload the solution and ensure Docker support is enabled in the IDE.

## Run Tests

Run from `src/`:

```bash
dotnet test ../test/unit/GtMotive.Estimate.Microservice.UnitTests/GtMotive.Estimate.Microservice.UnitTests.csproj --no-restore
dotnet test ../test/infrastructure/GtMotive.Estimate.Microservice.InfrastructureTests/GtMotive.Estimate.Microservice.InfrastructureTests.csproj --no-restore
dotnet test ../test/functional/GtMotive.Estimate.Microservice.FunctionalTests/GtMotive.Estimate.Microservice.FunctionalTests.csproj --no-restore
```

Functional tests use Testcontainers and require Docker running locally. They start a real MongoDB container as a single-node replica set and exercise the real HTTP host.

## Inspect MongoDB

Connect MongoDB Compass to:

```text
mongodb://localhost:27017/?directConnection=true
```

The application database is `estimate`.

## Notes

- The host fails fast if `Infrastructure:Provider` is missing or invalid.
- Mongo configuration is required only when the provider is `Mongo`.
- Functional tests clean the database between scenarios.
