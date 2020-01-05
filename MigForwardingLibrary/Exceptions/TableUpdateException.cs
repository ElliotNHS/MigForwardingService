using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MigForwardingLibrary.Exceptions
{
    class TableUpdateException:Exception
    {
        public string RawSql { get; set; }
           
        // Recommended minimum constructors
        public TableUpdateException() { }

        public TableUpdateException(string message)
            : base(message) { }

        public TableUpdateException(string message, string rawSql)
            : base(message)
        {
            RawSql = rawSql;
        }

        public TableUpdateException(
            string message, Exception innerException)
            : base(message, innerException) { }

        public TableUpdateException(
            string message, string rawSql, Exception innerException)
            : base(message, innerException)
        {
            RawSql = rawSql;
        }
    }
}
