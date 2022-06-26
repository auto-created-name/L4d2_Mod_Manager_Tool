﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using L4d2_Mod_Manager.Utility;

namespace L4d2_Mod_Manager.Domain.Repository
{
    public class ModRepository : IDisposable
    {
        private SQLiteConnection connection;
        public static ModRepository Instance { get; } = new ModRepository();

        private ModRepository()
        {
            connection = new SQLiteConnection("data source=mod.db");
            connection.Open();
            CreateModDBIfNotExists();
        }

        public Maybe<Mod> FindModById(int modId)
        {

            var command = connection.CreateCommand();
            command.CommandText = $"SELECT * FROM mod WHERE id={modId}";
            using var reader = command.ExecuteReader();
            if (reader.Read())
            {
                return CreateFromReader(reader);
            }
            else
            {
                return Maybe.None;
            }
        }

        /// <summary>
        /// 储存模组实体
        /// </summary>
        public Mod SaveMod(Mod mod)
        {
            var command = connection.CreateCommand();
            command.CommandText = $"INSERT INTO mod VALUES(" +
                $"NULL," +
                $"\"{mod.FileId}\", " +
                $"\"{mod.vpkId}\"," +
                $"\"{mod.Thumbnail}\", " +
                $"\"{mod.Title}\"," +
                $" \"{mod.Version}\", " +
                $"\"{mod.Tagline}\", " +
                $"\"{mod.Author}\"," +
                $"\"{mod.WorkshopTitle}\"," +
                $"\"{mod.WorkshopDescript}\"," +
                $"\"{mod.WorkshopPreviewImage}\");" +
                $"select last_insert_rowid();";
            long id = (long) command.ExecuteScalar();

            return mod with { Id = (int)id };
        }

        /// <summary>
        /// 更新模组
        /// </summary>
        public void UpdateMod(Mod mod)
        {
            string descriptBase64 = Convert.ToBase64String(Encoding.UTF8.GetBytes(mod.WorkshopDescript));

            var command = connection.CreateCommand();
            command.CommandText = $"UPDATE mod SET " +
                $"workshop_title = \"{mod.WorkshopTitle}\"," +
                $"workshop_descript = \"{descriptBase64}\"," +
                $"workshop_previewImage = \"{mod.WorkshopPreviewImage}\" " +
                $"WHERE " +
                $"id = {mod.Id}";
            command.ExecuteNonQuery();
        }

        public IEnumerable<Mod> GetMods()
        {
            var command = connection.CreateCommand();
            command.CommandText = "SELECT * FROM mod";
            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                var mod = CreateFromReader(reader);
                var base64Data = Convert.FromBase64String(mod.WorkshopDescript);
                var descript = Encoding.UTF8.GetString(base64Data);
                yield return mod with { WorkshopDescript = descript };
            }
        }

        /// <summary>
        /// 创建mod数据库
        /// </summary>
        private void CreateModDBIfNotExists()
        {
            var command = connection.CreateCommand();
            command.CommandText =
                "CREATE TABLE IF NOT EXISTS mod(" +
                "id INTEGER PRIMARY KEY AUTOINCREMENT" +
                ",file_id INTEGER NOT NULL" +
                ",vpk_id TEXT" +
                ",thumbnail TEXT" +
                ",title TEXT" +
                ",version TEXT" +
                ",tagline TEXT" +
                ",author TEXT" +
                ",workshop_title TEXT" +
                ",workshop_descript TEXT" +
                ",workshop_previewImage TEXT" +
                ",FOREIGN KEY(file_id) REFERENCES mod_file(id)" +
                ");";
            command.ExecuteNonQuery();
        }

        public void Dispose()
        {
            if(connection != null)
            {
                connection.Close();
                connection = null;
            }
        }

        private static Mod CreateFromReader(SQLiteDataReader reader)
        {
            return 
                new Mod(
                    reader.GetInt32(0),
                    reader.GetInt32(1),
                    reader.GetString(2),
                    reader.GetString(3),
                    reader.GetString(4),
                    reader.GetString(5),
                    reader.GetString(6),
                    reader.GetString(7),
                    reader.GetString(8),
                    reader.GetString(9),
                    reader.GetString(10)
                );
        }
    }
}
