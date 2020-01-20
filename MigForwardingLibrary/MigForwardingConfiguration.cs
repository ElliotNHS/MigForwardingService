using System;
using System.IO;
using System.Xml.Serialization;

namespace MigForwardingLibrary
{

    public class XmlFileToObject<T> where T : class, new()
    {
        public XmlFileToObject(string filePath)
        {

        }

        public static T Load(string filePath)
        {
            var reader = new StreamReader(filePath);
            var serializer = new XmlSerializer(typeof(T));
            var result = serializer.Deserialize(reader) as T;
            return result;
        }
    }


    [Serializable]
    [XmlRoot("MigForwardingSettings")]
    public class MigForwardingSettings
    {
        [XmlElement()]
        public bool IntergratedSecurity { get; set; }
        [XmlElement()]
        public string SqlUsername { get; set; }
        [XmlElement()]
        public string SqlPassword { get; set; }
        [XmlElement()]
        public string Catalog { get; set; }
        [XmlElement()]
        public string Schema { get; set; }
        [XmlElement()]
        public string Source { get; set; }
        [XmlElement()]
        public string TableName { get; set; }
    }

    public class MigForwardingConfiguration
    {
        public bool IntergratedSecurity { get; set; }
        public string SqlUsername { get; set; }
        public string SqlPassword { get; set; }
        public string Catalog { get; set; }
        public string Schema { get; set; }
        public string Source { get; set; }
        public string TableName { get; set; }

        public MigForwardingConfiguration()
        {

            if(File.Exists("settings.xml"))
            {
                LoadFromXml("settings.xml");

            } else if(File.Exists("settings.json"))
            {
                LoadFromJson("settings.json");
            } else
            {
                throw new Exception("Could not load configuration settings file");
                public void EventLog()
                {
                    WriteEntry(String,EventLog,ErrorEventArgs,);
                    string "Service has an error";
                }
            }

            Console.WriteLine("IntergratedSecurity = " + IntergratedSecurity);
            Console.WriteLine("SqlUsername = " + SqlUsername);
            Console.WriteLine("SqlPassword = " + SqlPassword);
            Console.WriteLine("Catalog = " + Catalog);
            Console.WriteLine("Schema = " + Schema);
            Console.WriteLine("Source = " + Source);
            Console.ReadKey();
        }

        private void LoadFromXml(string path)
        {
            var settings = XmlFileToObject<MigForwardingSettings>.Load(path);
            IntergratedSecurity = settings.IntergratedSecurity;
            SqlUsername = settings.SqlUsername;
            SqlPassword = settings.SqlPassword;
            Catalog = settings.Catalog;
            Schema = settings.Schema;
            Source = settings.Source;
            TableName = settings.TableName;
        }

        private void LoadFromJson(string path)
        {
            var c = new ConfigurationBuilder()
                .SetBasePath(System.IO.Directory.GetCurrentDirectory())
                .AddJsonFile(path, false)
                .Build();
        }
    }
}
