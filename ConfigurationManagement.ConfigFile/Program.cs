using System;
using System.Configuration;

namespace ConfigurationManagement.ConfigFile
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Reading App.Config...");

            var sectionGroup = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None).SectionGroups["payments"] as PaymentsConfig;

            var credentials = sectionGroup.Credentials; //or sectionGroup.Sections["credentials"] as PaymentsCredentialsConfig;
            var service = sectionGroup.Service; // or sectionGroup.Sections["service"] as PaymentsServiceConfig;
            
            Console.WriteLine($"Username :{credentials.Username}, Password : {credentials.Password}");
            Console.WriteLine($"Url :{service.Url}");

            Console.ReadLine();
        }
    }
}
