using System;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;

[assembly: CLSCompliant(false)]

namespace GtMotive.Estimate.Microservice.InfrastructureTests.Infrastructure
{
    public sealed class GenericInfrastructureTestServerFixture : IDisposable
    {
        public GenericInfrastructureTestServerFixture()
        {
            var hostBuilder = new WebHostBuilder()
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseEnvironment("IntegrationTest")
                .UseDefaultServiceProvider(static options => { options.ValidateScopes = true; })
                .ConfigureAppConfiguration(static (_, builder) => { builder.AddEnvironmentVariables(); })
                .UseStartup<Startup>();

            Server = new TestServer(hostBuilder);
        }

        public TestServer Server { get; }

        /// <inheritdoc />
        public void Dispose()
        {
            Server?.Dispose();
        }
    }
}
