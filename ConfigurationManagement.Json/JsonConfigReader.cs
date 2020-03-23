using Microsoft.Extensions.Configuration;
using System;
using System.IO;

namespace ConfigurationManagement.Json
{
    public class JsonConfigReader
    {
        // For illustration purposes oly
        public PaymentsConfig ReadPaymentsConfig()
        {
            var environmentName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", EnvironmentVariableTarget.User);

            var configBuilder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .AddJsonFile($"appsettings.{environmentName}.json", true, true);


            var config = configBuilder.Build();
            var appConfig = config.GetSection("payments").Get<PaymentsConfig>();

            return appConfig;
        }
    }
}
