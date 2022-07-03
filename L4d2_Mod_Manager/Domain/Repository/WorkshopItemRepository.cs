using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L4d2_Mod_Manager.Domain.Repository
{
    /// <summary>
    /// 创意工坊项存储库
    /// </summary>
    public class WorkshopItemRepository
    {

        private SQLiteConnection connection;

        public WorkshopItemRepository()
        {
            connection = new SQLiteConnection("data source=mod.db");
            connection.Open();
            CreateTableIfNotExists();
        }

        /// <summary>
        /// [副作用]创建表格
        /// </summary>
        private void CreateTableIfNotExists()
        {
            var command = connection.CreateCommand();
            command.CommandText = 
                "CREATE TABLE IF NOT EXISTS workshop_infomation(" +
                "id INTEGER PRIMARY KEY" +
                ",mod_id INTEGER NOT NULL" +
                ",title TEXT" +
                ",descript TEXT" +
                ",preview TEXT" +
                ",FOREIGN KEY(mod_id) REFERENCES mod(id)" +
                "); ";
        }
    }
}
