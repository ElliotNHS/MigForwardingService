using Microsoft.Extensions.Configuration;
using System;

namespace MigForwardingLibrary
{
    public class MigForwardingConfiguration
    {
        public bool IntergratedSecurity { get; set; }
        public string SqlUsername { get; set; }
        public string SqlPassword { get; set; }
        public string Catalog { get; set; }
        public string Schema { get; set; }
        public string Source { get; set; }

        public MigForwardingConfiguration()
        {
            var c = new ConfigurationBuilder()
                .AddJsonFile(@"C:\Users\wy6282\Desktop\MigForwardingService\MigTopShelfService\bin\Debug\settings.json", false)
                .Build();

            IntergratedSecurity = (c["intergratedSecurity"] == "true") ? true : false;
            SqlUsername = c["sqlUsername"];
            SqlPassword = c["sqlPassword"];
            Catalog = c["catalog"];
            Schema = c["schema"];
            Source = c["source"];

            Console.WriteLine("IntergratedSecurity = " + IntergratedSecurity);
            Console.WriteLine("SqlUsername = " + SqlUsername);
            Console.WriteLine("SqlPassword = " + SqlPassword);
            Console.WriteLine("Catalog = " + Catalog);
            Console.WriteLine("Schema = " + Schema);
            Console.WriteLine("Source = " + Source);
            Console.ReadKey();
        }
    }
}
