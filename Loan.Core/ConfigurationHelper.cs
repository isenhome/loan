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

        public static string SqlDB
        {
            get
            {
                return ConfigurationManager.ConnectionStrings["SqlDB"].ToString();
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
