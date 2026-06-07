using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using Testcontainers.MongoDb;
using Xunit;

[assembly: CLSCompliant(false)]

namespace GtMotive.Estimate.Microservice.FunctionalTests.Infrastructure
{
    public sealed class CompositionRootTestFixture : IDisposable, IAsyncLifetime
    {
        private bool disposed;

        private MongoDbContainer mongoDbContainer = default!;

        private MongoClient mongoClient = default!;

        private string databaseName = default!;

        public IConfiguration Configuration { get; private set; } = default!;

        public TestServer Server { get; private set; } = default!;

        public async Task InitializeAsync()
        {
            mongoDbContainer = new MongoDbBuilder()
                .WithUsername(string.Empty)
                .WithPassword(string.Empty)
                .Build();

            await mongoDbContainer.StartAsync().ConfigureAwait(false);

            databaseName = $"functionaltests_{Guid.NewGuid():N}";
            mongoClient = new MongoClient(mongoDbContainer.GetConnectionString());

            Configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddEnvironmentVariables()
                .AddInMemoryCollection(new Dictionary<string, string>
                {
                    ["MongoDb:ConnectionString"] = mongoDbContainer.GetConnectionString(),
                    ["MongoDb:MongoDbDatabaseName"] = databaseName,
                })
                .Build();

            var hostBuilder = new WebHostBuilder()
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseEnvironment("IntegrationTest")
                .UseDefaultServiceProvider(static options => { options.ValidateScopes = true; })
                .ConfigureAppConfiguration((_, builder) => { builder.AddConfiguration(Configuration); })
                .UseStartup<Startup>();

            Server = new TestServer(hostBuilder);
        }

        public async Task DisposeAsync()
        {
            if (disposed)
            {
                return;
            }

            disposed = true;

            Server?.Dispose();
            mongoClient?.Dispose();

            if (mongoDbContainer is not null)
            {
                await mongoDbContainer.DisposeAsync().ConfigureAwait(false);
            }
        }

        public async Task ResetDatabaseAsync()
        {
            if (mongoClient is null)
            {
                return;
            }

            await mongoClient.DropDatabaseAsync(databaseName).ConfigureAwait(false);
        }

        public async Task UsingHandlerForRequest<TRequest>(Func<IRequestHandler<TRequest, Unit>, Task> handlerAction)
            where TRequest : IRequest
        {
            ArgumentNullException.ThrowIfNull(handlerAction);

            using var scope = Server.Services.CreateScope();
            var handler = scope.ServiceProvider.GetRequiredService<IRequestHandler<TRequest, Unit>>();

            await handlerAction.Invoke(handler).ConfigureAwait(false);
        }

        public async Task UsingHandlerForRequestResponse<TRequest, TResponse>(Func<IRequestHandler<TRequest, TResponse>, Task> handlerAction)
            where TRequest : IRequest<TResponse>
        {
            ArgumentNullException.ThrowIfNull(handlerAction);

            using var scope = Server.Services.CreateScope();
            var handler = scope.ServiceProvider.GetRequiredService<IRequestHandler<TRequest, TResponse>>();

            await handlerAction.Invoke(handler).ConfigureAwait(false);
        }

        public async Task UsingRepository<TRepository>(Func<TRepository, Task> handlerAction)
        {
            ArgumentNullException.ThrowIfNull(handlerAction);

            using var scope = Server.Services.CreateScope();
            var handler = scope.ServiceProvider.GetRequiredService<TRepository>();

            await handlerAction.Invoke(handler).ConfigureAwait(false);
        }

        public void Dispose()
        {
            DisposeAsync().GetAwaiter().GetResult();
        }
    }
}
