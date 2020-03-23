using System;
using System.Threading;

namespace ConfigurationManagement.External
{
    class Program
    {
        static void Main(string[] args)
        {
            var environment = "development";
            Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", environment);

            Console.WriteLine("Hello World!");

            var storeSettings = new FakeConfigStore(environment);
            var configManager = new ConfigManager<string>(storeSettings, TimeSpan.FromSeconds(15));
            configManager.StartMonitor();

            var settings = configManager.GetSettings<PaymentsConfig>();

            Console.WriteLine($"Username :{settings.Credentials.Username}, Password : {settings.Credentials.Password}");
            Console.WriteLine($"Url :{settings.Service.Url}");

            Thread.Sleep(120000);

            Console.ReadLine();

            Environment.SetEnvironmentVariable("Paypal_URL", environment);
            to read
            Environment.GetEnvironmentVariable("Paypal_URL");

        }
    }
}
