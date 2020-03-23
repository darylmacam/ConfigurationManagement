using System;

namespace ConfigurationManagement.Json.DotNetFramework
{
    class Program
    {
        static void Main(string[] args)
        {
            Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", "production");

            Console.WriteLine("Reading appsettings.json from .Net Framework Console..." + Environment.NewLine);

            var jsonConfigReader = new JsonConfigReader();
            var config = jsonConfigReader.ReadPaymentsConfig();

            Console.WriteLine($"Username :{config.Credentials.Username}, Password : {config.Credentials.Password}");
            Console.WriteLine($"Url :{config.Service.Url}");

            Console.ReadLine();
        }
    }
}
