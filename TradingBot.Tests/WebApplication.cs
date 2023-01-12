using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using TradingBot.Tests.XUnitUtilities;
using Xunit.Abstractions;

namespace TradingBot.Tests
{
    public class WebApplication : WebApplicationFactory<Program>
    {
        private readonly ITestOutputHelper _testOutputHelper;

        public WebApplication(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;
        }

        protected override IHost CreateHost(IHostBuilder builder)
        {
            builder.UseEnvironment("Development");
            builder.ConfigureLogging(logBuilder =>
            {
                logBuilder.Services.AddSingleton<ILoggerProvider>(serviceProvider => new XUnitLoggerProvider(_testOutputHelper));
            });

            return base.CreateHost(builder);
        }
    }
}
