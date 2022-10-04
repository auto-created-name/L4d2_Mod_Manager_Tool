using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using Dapper;
using Dapper.Contrib.Extensions;
using System.Data;

namespace Infrastructure
{
    public class DBConnection : IDisposable
    {
        private IDbConnection connection;
        public DBConnection(IDbConnection connection)
        {
            this.connection = connection;
            connection.Open();
        }
        public void Dispose()
        {
            connection.Dispose();
        }

        public DBTransaction BeginTransaction()
            => new(connection, connection.BeginTransaction());

        public long Insert<T>(T po) where T : class
            => connection.Insert(po);
    }

    public class DBTransaction : IDisposable
    {
        private IDbConnection connection;
        private IDbTransaction transaction;
        public DBTransaction(IDbConnection connection, IDbTransaction transaction)
        {
            this.connection = connection;
            this.transaction = transaction;
        }

        public void Dispose()
        {
            transaction.Commit();
        }
        public long Insert<T>(T po) where T : class
            => connection.Insert(po, transaction);

        public void Update<T>(T po) where T : class
            => connection.Update(po, transaction);
    }

    public class DapperHelper
    {
        private const string ConnectString = "data source = mod2.db";
        private const string TableString =
            "CREATE TABLE IF NOT EXISTS mod_file("
            + "id INTEGER PRIMARY KEY"
            + ",vpk_id INTEGER NOT NULL"
            + ",file_name TEXT NOT NULL UNIQUE"
            + ",localinfo_id INTEGER"
            + ",workshopinfo_id INTEGER"
            + "); "
            + "CREATE TABLE IF NOT EXISTS mod("
            + "id INTEGER PRIMARY KEY AUTOINCREMENT"
            + ",thumbnail TEXT"
            + ",title TEXT"
            + ",version TEXT"
            + ",tagline TEXT"
            + ",author TEXT"
            + ",description TEXT"
            + ",categories TEXT"
            + ");" 
            + "CREATE TABLE IF NOT EXISTS workshopinfo("
            + "vpk_id PRIMARY KEY" 
            + ",preview TEXT"
            + ",title TEXT"
            + ",description TEXT"
            + ",tags TEXT"
            + ");";

        public static DapperHelper Instance { get; } = new DapperHelper();

        private DapperHelper()
        {
            using var conn = new SQLiteConnection(ConnectString);
            conn.Execute(TableString);
        }

        public static DBConnection OpenConnection()
            => new(new SQLiteConnection(ConnectString));

        public IEnumerable<T> Query<T>(string sql)
        {
            using var conn = new SQLiteConnection(ConnectString);
            return conn.Query<T>(sql);
        }

        public bool Update<T>(T po) where T : class
        {
            using var conn = new SQLiteConnection(ConnectString);
            return conn.Update<T>(po);
        }

        public IEnumerable<T> GetAll<T>(IDbTransaction transaction = null, int? timeout = null) where T : class
        {
            using var conn = new SQLiteConnection(ConnectString);
            return conn.GetAll<T>(transaction, timeout);
        }

        public long Insert<T>(T po, IDbTransaction transaction = null, int? timeout = null) where T : class
        {
            using var conn = new SQLiteConnection(ConnectString);
            return conn.Insert(po, transaction, timeout);
        }

        public IEnumerable<TOut> JoinQuery<T1, T2, TOut>(string sql, Func<T1, T2, TOut> mapper)
        {
            using var conn = new SQLiteConnection(ConnectString);
            return conn.Query<T1, T2, TOut>(sql, mapper);
        }

        public T Get<T>(int id) where T : class
        {
            using var conn = new SQLiteConnection(ConnectString);
            return conn.Get<T>(id);
        }
    }
}
