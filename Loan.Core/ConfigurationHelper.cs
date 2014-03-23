using System;
using System.Collections.Generic;
using System.Configuration;

namespace Loan.Core
{
    public static class ConfigurationHelper
    {
        public static String DBConnectionString
        {
            get
            {
                return ConfigurationManager.ConnectionStrings["DBConnectionString"].ToString();
            }
        }

        public static string SqliteDB
        {
            get
            {
                return ConfigurationManager.ConnectionStrings["SqliteDB"].ToString();
            }
        }

        public static string MySqlDB
        {
            get
            {
                return ConfigurationManager.ConnectionStrings["MySqlDB"].ToString();
            }
        }

        public static string DBPath
        {
            get
            {
                return ConfigurationManager.AppSettings["DBPath"].TryString();
            }
        }
    }
}
