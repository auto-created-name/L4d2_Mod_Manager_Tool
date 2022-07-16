using L4d2_Mod_Manager_Tool.Utility;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L4d2_Mod_Manager_Tool.Domain.Repository
{
    /// <summary>
    /// 创意工坊项存储库
    /// </summary>
    public class ModFileRepository
    {
        public const int TempModFileId = -1;
        private SQLiteConnection connection;

        public ModFileRepository()
        {
            connection = new SQLiteConnection("data source=mod.db");
            connection.Open();
            CreateTableIfNotExists();
        }

        /// <summary>
        /// 通过文件名查找文件
        /// </summary>
        public Maybe<ModFile> FindModFileByFileName(string fn)
        {
            var cmd = command($"SELECT * FROM mod_file WHERE filename=\"{fn}\"");
            using var reader = cmd.ExecuteReader();
            return ReadModFile(reader);
        }

        public Maybe<ModFile> FindModFileById(int id)
        {
            var command = connection.CreateCommand();
            command.CommandText =
                $"SELECT * FROM mod_file WHERE id={id};";
            using var reader = command.ExecuteReader();
            return ReadModFile(reader);
        }

        private static Maybe<ModFile> ReadModFile(SQLiteDataReader reader)
        {
            if (reader.Read())
            {
                return new ModFile(
                    (int)reader.GetInt64(0),
                    reader.GetString(1),
                    reader.GetBoolean(2)
                );
            }
            else
            {
                return Maybe.None;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public bool ModFileExists(string file)
        {
            var res = (long)command($"SELECT COUNT (*) FROM mod_file WHERE filename=\"{file}\";")
                .ExecuteScalar();
            return res > 0;
        }

        public ModFile SaveModFile(ModFile mf)
        {
            if(mf.Id == TempModFileId)
            {
                var command = connection.CreateCommand();
                command.CommandText =
                    $"INSERT INTO mod_file VALUES(" +
                    $"NULL" +
                    $",\"{mf.FilePath}\"" +
                    $",\"{(mf.Actived ? 1 : 0)}\"" +
                    $");select last_insert_rowid();";
                try
                {
                    long id = (long)command.ExecuteScalar();
                    return mf with { Id = (int)id };
                }
                catch
                {
                    return mf;
                }
            }
            return mf;
        }

        /// <summary>
        /// 更新模组文件信息
        /// </summary>
        public void UpdateModFile(ModFile mf)
        {

            var command = connection.CreateCommand();
            command.CommandText =
                $"UPDATE mod_file SET " +
                $"filename = \"{mf.FilePath}\"" +
                $",active = {(mf.Actived ? 1 : 0)} " +
                $"WHERE id = {mf.Id};";
            int effectRow = command.ExecuteNonQuery();
            if (effectRow == 0) throw new Exception("更新ModFile失败");
        }

        /// <summary>
        /// [副作用]创建表格
        /// </summary>
        private void CreateTableIfNotExists()
        {
            var command = connection.CreateCommand();
            command.CommandText =
                "CREATE TABLE IF NOT EXISTS mod_file(" +
                "id INTEGER PRIMARY KEY" +
                ",filename TEXT NOT NULL UNIQUE" +
                ",active INT NOT NULL); ";
            command.ExecuteNonQuery();
        }

        private SQLiteCommand command(string cmd)
        {
            var command = connection.CreateCommand();
            command.CommandText = cmd;
            return command;
        }
    }
}
