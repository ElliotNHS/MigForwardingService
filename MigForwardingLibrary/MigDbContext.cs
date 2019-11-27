using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MigForwardingLibrary
{
    public class MigDbContext
    {
        private readonly MigForwardingConfiguration Config;

        private readonly string ConnectionString;

        public MigDbContext(MigForwardingConfiguration config)
        {
            Config = config;
            ConnectionString = BuildConnectionString();
        }
        public void Upgrade() {

            try
            {
                var dbConnection = new SqlConnection(ConnectionString);
                var queryStatement = @"  
                    ALTER TABLE log_LPRES_ATNA_Simplified ADD [MaywoodsID]	BIGINT IDENTITY (1,1);
                    ALTER TABLE log_LPRES_ATNA_Simplified ADD [MaywoodsDateTime] DATETIME;
                    ALTER TABLE log_LPRES_ATNA_Simplified ADD [MaywoodsAuditID] BIGINT;
                    GO

                    ALTER TABLE log_LPRES_ATNA_Simplified ADD CONSTRAINT PK_MaywoodsID PRIMARY KEY(MaywoodsID);

                    IF EXISTS ( SELECT NAME FROM dbo.sysindexes WHERE name = 'idx_MaywoodsDateTime')
                    DROP INDEX [log_LPRES_ATNA_Simplified].[idx_MaywoodsDateTime]
                    GO

                    IF NOT EXISTS ( SELECT NAME FROM dbo.sysindexes WHERE name = 'idx_MaywoodsDateTime')
                    CREATE INDEX idx_MaywoodsDateTime ON [log_LPRES_ATNA_Simplified] (MaywoodsDateTime)
                    GO";

                using (SqlCommand _cmd = new SqlCommand(queryStatement, dbConnection))
                {
                    dbConnection.Open();
                    _cmd.ExecuteNonQuery();
                    dbConnection.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Failed to upgrade the MIG forwarding table");
                Console.WriteLine(ex.Message);
                Console.ReadKey();
            }
            

        }
        public void Downgrade() { }

        public DataTable SelectTop()
        {
            var dbConnection = new SqlConnection(ConnectionString);
            var queryStatement = @"SELECT TOP (1000) [EventDateTime]
                          ,[EventType]
                          ,[NHSNumber]
                          ,[StateID]
                          ,[UserID]
                          ,[DocumentUUID]
                          ,[DocumentTitle]
                          ,[ClientIP]
                       FROM[" + Config.Catalog + "].[" + Config.Schema + "].[" + Config.TableName + "]";
            
            var dataTable = new DataTable();

            using (SqlCommand _cmd = new SqlCommand(queryStatement, dbConnection))
            {
                dbConnection.Open();
                var adapter = new SqlDataAdapter(_cmd);
                adapter.Fill(dataTable);
                dbConnection.Close();
            }
            return dataTable;
        }

        private string BuildConnectionString()
        {
            // Initialize the connection string builder for the
            // underlying provider.
            SqlConnectionStringBuilder sqlBuilder = new SqlConnectionStringBuilder();

            // Set the properties for the data source.
            sqlBuilder.DataSource = Config.Source;
            sqlBuilder.InitialCatalog = Config.Catalog;
            sqlBuilder.MultipleActiveResultSets = true;
            sqlBuilder.ApplicationName = "EntityFramework";

            if (!Config.IntergratedSecurity)
            {
                sqlBuilder.UserID = Config.SqlUsername;
                sqlBuilder.Password = Config.SqlPassword;
            }

            sqlBuilder.IntegratedSecurity = Config.IntergratedSecurity;

            // Build the SqlConnection connection string.
            string providerString = sqlBuilder.ToString();
            Console.WriteLine(providerString);
            Console.ReadKey();

            return providerString;

        }

    }
}
