using Microsoft.Extensions.Configuration;
using System;
using System.IO;

namespace ConfigurationManagement.Yaml.ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Reading appsettings.yaml from .Net Core Console..." + Environment.NewLine);

            var environmentName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            var configBuilder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddYamlFile("appsettings.yaml")
                .AddYamlFile($"appsettings.{environmentName}.yaml", true, true);

            var configRoot = configBuilder.Build();
            var config = configRoot.GetSection("payments").Get<PaymentsConfig>();

            Console.WriteLine($"Username :{config.Credentials.Username}, Password : {config.Credentials.Password}");
            Console.WriteLine($"Url :{config.Service.Url}");

            Console.ReadLine();
        }
    }
}

