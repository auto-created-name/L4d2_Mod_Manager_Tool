using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L4d2_Mod_Manager_Tool.Domain.Repository
{
    public abstract class SQLiteDatabaseRepositoryBase : IDisposable
    {
        private SQLiteConnection connection;
        public SQLiteDatabaseRepositoryBase(string file)
        {
            connection = new SQLiteConnection($"data source={file}");
            connection.Open();
            CreateDatabase();
        }

        protected abstract void CreateDatabase();

        protected int ExecuteNonQuery(string cmd)
            => CreateCommand(cmd).ExecuteNonQuery();

        protected SQLiteDataReader ExecuteQueryReader(string cmd)
            => CreateCommand(cmd).ExecuteReader();

        protected SQLiteCommand CreateCommand(string cmd)
        {
            var command = connection.CreateCommand();
            command.CommandText = cmd;
            return command;
        }

        public void Dispose()
        {
            connection.Close();
        }
    }
}
