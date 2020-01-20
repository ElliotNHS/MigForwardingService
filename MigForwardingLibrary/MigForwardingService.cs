using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Threading;
using System.Data.SqlClient;
using System.Data.Entity.Core.EntityClient;
using System.Data;
using System.Diagnostics;

namespace MigForwardingLibrary
{
    public class MigForwardingService
    {
        private SemaphoreSlim _semaphoreSlim;
     
        public MigForwardingService() {
        }

        public void Start() 
        {
            // Create an EventLog instance and assign its source.

            EventLog Startlog = new EventLog("MyStartLog");
            Startlog.Source = "MyNewLogSource";

            // Write an informational entry to the event log.    
            Startlog.WriteEntry("Service has started.", EventLogEntryType.Information);

            var config = new MigForwardingConfiguration();

            var dbContext = new MigDbContext(config);
            dbContext.Upgrade();

            Console.ReadKey();
            dbContext.Downgrade();

            _semaphoreSlim = new SemaphoreSlim(0);
            while(true)

            {
                // Create an EventLog instance and assign its source.
                EventLog Batchlog = new EventLog("MyBatchLog");
                Batchlog.Source = "MyNewLogSource";

                // Write an informational entry to the event log.    
                Batchlog.WriteEntry("Service is waking up and processing the next 50 records.", EventLogEntryType.Information);


                 Console.WriteLine("It is {0} and all is well.", DateTime.Now); 
                var result = dbContext.SelectTop50();
                foreach (DataRow dataRow in result.Rows)
                {
                    foreach (var item in dataRow.ItemArray)
                    {
                        Console.WriteLine(item);
                    }
                }
                Thread.Sleep(3000);

                if (_semaphoreSlim.Wait(1000))
                {
                    OnStopping();
                    break;
                }
            }
        }
        public void Stop() {
            _semaphoreSlim.Release();

        }
        public void OnStopping()
        {
            Console.WriteLine("MIG forwarding service is stopping.");
            Console.WriteLine("Press any key to exit the program.");
            Console.ReadKey();
            // Here any connections would be closed, log files would be finished, 
            // release any references to unmanaged code, any clean up activity
            // Create an EventLog instance and assign its source.

            EventLog Stoplog = new EventLog("MyStopLog");
            Stoplog.Source = "MyNewLogSource";

            // Write an informational entry to the event log.    
            Stoplog.WriteEntry("Service has stopped.", EventLogEntryType.Information);
        }

      

        private string BuildConnectionString(MigForwardingConfiguration config)
        {
            // Initialize the connection string builder for the
            // underlying provider.
            SqlConnectionStringBuilder sqlBuilder = new SqlConnectionStringBuilder
            {

                // Set the properties for the data source.
                DataSource = config.Source,
                InitialCatalog = config.Catalog,
                MultipleActiveResultSets = true,
                ApplicationName = "EntityFramework"
            };

            if (!config.IntergratedSecurity)
            {
                sqlBuilder.UserID = config.SqlUsername;
                sqlBuilder.Password = config.SqlPassword;
            }
            
            sqlBuilder.IntegratedSecurity = config.IntergratedSecurity;

            // Build the SqlConnection connection string.
            string providerString = sqlBuilder.ToString();
            Console.WriteLine(providerString);   
            Console.ReadKey();

            return providerString;

            // Initialize the EntityConnectionStringBuilder.
            EntityConnectionStringBuilder entityBuilder =
                new EntityConnectionStringBuilder();

            //Set the provider name.
            //entityBuilder.Provider = providerName;

            // Set the provider-specific connection string.
            //entityBuilder.ProviderConnectionString = providerString;

            // Set the Metadata location.
            //entityBuilder.Metadata = @"res://*/AdventureWorksModel.csdl|
            //                res://*/AdventureWorksModel.ssdl|
            //                res://*/AdventureWorksModel.msl";
            //Console.WriteLine(entityBuilder.ToString());

            using (EntityConnection conn =
                new EntityConnection(entityBuilder.ToString()))
            {
                conn.Open();
                Console.WriteLine("Just testing the connection.");
                conn.Close();
            }
        }
    }
}
