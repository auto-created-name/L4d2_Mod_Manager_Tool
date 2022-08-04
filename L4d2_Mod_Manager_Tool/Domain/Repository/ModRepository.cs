﻿using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using L4d2_Mod_Manager_Tool.Utility;
using System.Web;

namespace L4d2_Mod_Manager_Tool.Domain.Repository
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

        public void Initialize()
        {
            CreateModDBIfNotExists();
        }

        public Maybe<Mod> FindModByFileId(int fid)
        {
            var command = connection.CreateCommand();
            command.CommandText = $"SELECT * FROM mod WHERE file_id={fid}";
            using var reader = command.ExecuteReader();
            return CreateFromReaderWithRead(reader);
        }

        public Maybe<Mod> FindModById(int modId)
        {

            var command = connection.CreateCommand();
            command.CommandText = $"SELECT * FROM mod WHERE id={modId}";
            using var reader = command.ExecuteReader();
            return CreateFromReaderWithRead(reader);
        }

        /// <summary>
        /// 储存模组实体
        /// </summary>
        public Mod SaveMod(Mod mod)
        {
            var command = connection.CreateCommand();
            command.CommandText = $"INSERT INTO mod VALUES(" +
                $"NULL" +
                $",\"{mod.FileId}\"" +
                $",\"{mod.vpkId}\"" +
                $",\"{mod.Thumbnail}\"" +
                $",\"{mod.Title}\"" +
                $",\"{mod.Version}\"" +
                $",\"{mod.Tagline}\"" +
                $",\"{mod.Author}\"" +
                $",\"{mod.Description}\"" +
                $",\"{mod.CategoriesSingleLine()}\"" +
                $",\"{HttpUtility.UrlEncode(mod.WorkshopTitle)}\"" +
                $",\"{mod.WorkshopDescript}\"" +
                $",\"{mod.WorkshopPreviewImage}\"" +
                $",\"{mod.TagsSingleLine()}\");" +
                $"select last_insert_rowid();";
            long id = (long) command.ExecuteScalar();

            return mod with { Id = (int)id };
        }

        /// <summary>
        /// 更新模组
        /// </summary>
        public bool UpdateMod(Mod mod)
        {
            string descriptBase64 = Convert.ToBase64String(Encoding.UTF8.GetBytes(mod.WorkshopDescript));

            var command = connection.CreateCommand();
            command.CommandText = $"UPDATE mod SET " +
                $"workshop_title = \"{HttpUtility.UrlEncode(mod.WorkshopTitle)}\"" +
                $",workshop_descript = \"{descriptBase64}\"" +
                $",workshop_previewImage = \"{mod.WorkshopPreviewImage}\"" +
                $",workshop_tags = \"{mod.TagsSingleLine()}\"" +
                $" WHERE " +
                $"id = {mod.Id}";
            int res = command.ExecuteNonQuery();
            return res > 0;
        }

        public IEnumerable<Mod> GetMods()
        {
            var command = connection.CreateCommand();
            command.CommandText = "SELECT * FROM mod";
            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                yield return CreateFromReader(reader);
                //var base64Data = Convert.FromBase64String(mod.WorkshopDescript);
                //var descript = Encoding.UTF8.GetString(base64Data);
                //yield return mod with { WorkshopDescript = descript };
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
                ",description TEXT" +
                ",categories TEXT" +
                ",workshop_title TEXT" +
                ",workshop_descript TEXT" +
                ",workshop_previewImage TEXT" +
                ",workshop_tags TEXT" +
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

        private static Maybe<Mod> CreateFromReaderWithRead(SQLiteDataReader reader) {
            if (reader.Read())
                return CreateFromReader(reader);
            else
                return Maybe.None;
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
                    SplitString(reader.GetString(9), ','),
                    HttpUtility.UrlDecode(reader.GetString(10)),
                    DecodeBase64(reader.GetString(11)),
                    reader.GetString(12),
                    SplitString(reader.GetString(13), ',')
                );
        }

        private static string DecodeBase64(string str)
        {
            var data = Convert.FromBase64String(str);
            return Encoding.UTF8.GetString(data);
        }

        private static ImmutableArray<string> SplitString(string str, char separator)
        {
            if (string.IsNullOrEmpty(str))
                return ImmutableArray<string>.Empty;
            else
                return str.Split(separator).ToImmutableArray();
        }
    }
}
