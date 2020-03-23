using System;

namespace ConfigurationManagement.Json.DotNetCore
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Reading appsettings.json from .Net Core Console..." + Environment.NewLine);

            var jsonConfigReader = new JsonConfigReader();
            var config = jsonConfigReader.ReadPaymentsConfig();

            Console.WriteLine($"Username :{config.Credentials.Username}, Password : {config.Credentials.Password}");
            Console.WriteLine($"Url :{config.Service.Url}");

            Console.ReadLine();
        }
    }
}
