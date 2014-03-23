using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Loan.Core;
using System.IO;
using System.Data.SQLite;
using System.Data.Sql;
using System.Data.Common;
using System.Data.SqlClient;

namespace Loan.Data
{
    public class BaseDao
    {
        private PetaPoco.Database _db = null;
        public PetaPoco.Database database
        {
            get
            {
                if (_db == null)
                {
                    _db = new PetaPoco.Database(ConfigurationHelper.SqlDB, sqliteProvider);
                }
                return _db;
            }
        }

        private new DbProviderFactory sqliteProvider
        {
            get
            {
                //return DbProviderFactories.GetFactory(new SQLiteConnection(ConfigurationHelper.SqliteDB));
                return DbProviderFactories.GetFactory(new SqlConnection(ConfigurationHelper.SqlDB));
            }
        }
    }
}
