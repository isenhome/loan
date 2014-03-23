using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Loan.Core;
using System.IO;
using System.Data.Sql;
using System.Data.Common;
using MySql.Data.MySqlClient;

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
                    _db = new PetaPoco.Database("MySqlDB");
                }
                return _db;
            }
        }

        private DbProviderFactory MySqlProvider
        {
            get
            {
                return DbProviderFactories.GetFactory(new MySqlConnection(ConfigurationHelper.MySqlDB));
            }
        }
    }
}
