using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Data.SqlClient;
using Dapper;
using System.Reflection;
using System.Configuration;
using System.Data.Common;
using Oracle.ManagedDataAccess.Client;

namespace CAMPOracleDataAccess.Data.Utility
{
    public class ConnectionProvider
    {
        DbConnection conn;
        string connectionString;
        DbProviderFactory factory;

        // Constructor that retrieves the connectionString from the config file
        public ConnectionProvider()
        {
            this.connectionString = ConfigurationManager.ConnectionStrings[0].ConnectionString.ToString();
            factory = DbProviderFactories.GetFactory(ConfigurationManager.ConnectionStrings[0].ProviderName.ToString());
        }

        // Constructor that accepts the connectionString and Database ProviderName i.e SQL or Oracle
        public ConnectionProvider(string connectionString, string connectionProviderName)
        {
            this.connectionString = connectionString;
            factory = DbProviderFactories.GetFactory(connectionProviderName);
        }

        // Only inherited classes can call this.
        public DbConnection GetOpenConnection()
        {
            conn = factory.CreateConnection();
            conn.ConnectionString = this.connectionString;
            conn.Open();

            return conn;
        }

    }
    public class BaseUtility
    {
        private static string _connectionString =
            Encryption.Decrypt(ConfigurationManager.ConnectionStrings["OracleConnection"].ConnectionString);

        private static OracleConnection db;
        public static string ConnectionString
        {
            get
            {
                return _connectionString;
            }
        }

        public static OracleConnection OpenConnection()
        {
            db = new OracleConnection(ConnectionString);
            db.Open();
            return db;
        }
    }

}
