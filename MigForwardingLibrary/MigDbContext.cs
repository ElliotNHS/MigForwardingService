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

        public void ExecuteNonQuery(string queryStatement)
        {
            var dbConnection = new SqlConnection(ConnectionString);
            using (SqlCommand _cmd = new SqlCommand(queryStatement, dbConnection))
            {
                dbConnection.Open();
                _cmd.ExecuteNonQuery();
                dbConnection.Close();
            }
        }
        public DataTable SelectAndFill(string queryStatement)
        {
            var dbConnection = new SqlConnection(ConnectionString);
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

        public void Upgrade() {

            try
            {

                ExecuteNonQuery(@"ALTER TABLE log_LPRES_ATNA_Simplified ADD[MaywoodsID]  BIGINT IDENTITY(1, 1);");
                ExecuteNonQuery(@"ALTER TABLE log_LPRES_ATNA_Simplified ADD[MaywoodsDateTime] DATETIME;");
                ExecuteNonQuery(@"ALTER TABLE log_LPRES_ATNA_Simplified ADD[MaywoodsAuditID] BIGINT;");

                ExecuteNonQuery(@"ALTER TABLE log_LPRES_ATNA_Simplified ADD CONSTRAINT PK_MaywoodsID PRIMARY KEY(MaywoodsID);");
                ExecuteNonQuery(@"IF EXISTS ( SELECT NAME FROM dbo.sysindexes WHERE name = 'idx_MaywoodsDateTime') 
DROP INDEX [log_LPRES_ATNA_Simplified].[idx_MaywoodsDateTime]");


                ExecuteNonQuery(@"IF NOT EXISTS ( SELECT NAME FROM dbo.sysindexes WHERE name = 'idx_MaywoodsDateTime') 
CREATE INDEX idx_MaywoodsDateTime ON [log_LPRES_ATNA_Simplified] (MaywoodsDateTime)");


            }
            catch (Exception ex)
            {
                Console.WriteLine("Failed to upgrade the MIG forwarding table");
                Console.WriteLine(ex.Message);
                Console.ReadKey();
            }
            

        }
        //Check when database was last updated?
        //select * from sys.objects
        //order by modify_date desc
        public void Downgrade() {
            try
            {


             ExecuteNonQuery(@"IF EXISTS(SELECT NAME FROM dbo.sysindexes WHERE name = 'idx_MaywoodsDateTime')
                             DROP INDEX[log_LPRES_ATNA_Simplified].[idx_MaywoodsDateTime]");           
             ExecuteNonQuery(@"ALTER TABLE log_LPRES_ATNA_Simplified DROP CONSTRAINT PK_MaywoodsID;");
             ExecuteNonQuery(@"ALTER TABLE log_LPRES_ATNA_Simplified DROP COLUMN[MaywoodsID];"); 
             ExecuteNonQuery(@"ALTER TABLE log_LPRES_ATNA_Simplified DROP COLUMN[MaywoodsDateTime] ;");
             ExecuteNonQuery(@"ALTER TABLE log_LPRES_ATNA_Simplified DROP COLUMN[MaywoodsAuditID] ;");

     
               

            }
            catch (Exception ex)
            {
                Console.WriteLine("Failed to downgrade the MIG forwarding table");
                Console.WriteLine(ex.Message);
                Console.ReadKey();
            }

        }

        public DataTable SelectTop50()
        {

            var queryStatement = @"  SELECT TOP (50)
                   [EventDateTime]
                      ,[EventType]
                      ,[NHSNumber]
                      ,[StateID]
                      ,[UserID]
                      ,[DocumentUUID]
                      ,[DocumentTitle]
                      ,[ClientIP]
                      ,[MaywoodsID]
                      ,[MaywoodsDateTime]
                      ,[MaywoodsAuditID]
                       FROM[" + Config.Catalog + "].[" + Config.Schema + "].[" + Config.TableName + "]	  " +
                       "WHERE [MaywoodsDateTime] IS NULL AND  [MaywoodsAuditID] IS NULL      ORDER BY[EventDateTime] ";


            return SelectAndFill(queryStatement);
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
