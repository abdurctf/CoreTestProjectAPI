using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace DBContext
{
    public class DatabaseContextReadOnly
    {
        public string ReadConn { get; }
        public IDbConnection Db { get; }
        public DatabaseContextReadOnly(string readConn)
        {
            ReadConn = readConn;
            Db = GetDbConnection();
        }
        public IDbConnection GetDbConnection()
        {
            var conn = new OracleConnection(ReadConn);
            if (conn.State == ConnectionState.Closed)
            {
                conn.Open();
            }
            OracleGlobalization info = conn.GetSessionInfo();
            info.TimeZone = "America/New_York";
            conn.SetSessionInfo(info);
            Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;
            return conn;
        }

        public void CloseConnection(DatabaseContextReadOnly conn)
        {
            if (conn.Db.State == ConnectionState.Open || conn.Db.State == ConnectionState.Broken)
            {
                conn.Db.Close();
            }
        }
    }
}
