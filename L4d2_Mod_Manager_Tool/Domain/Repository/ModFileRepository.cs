using L4d2_Mod_Manager_Tool.Utility;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading;
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
        private ReaderWriterLock rwlock = new();

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
            return WithReaderLock(() =>
            {
                var cmd = Command($"SELECT * FROM mod_file WHERE filename=\"{fn}\"");
                using var reader = cmd.ExecuteReader();
                return ReadModFile(reader);
            });
        }

        public Maybe<ModFile> FindModFileById(int id)
        {
            return WithReaderLock(() => {
                var command = connection.CreateCommand();
                command.CommandText =
                    $"SELECT * FROM mod_file WHERE id={id};";
                using var reader = command.ExecuteReader();
                return ReadModFile(reader);
            });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public bool ModFileExists(string file)
        {
            var res = (long)Command($"SELECT COUNT (*) FROM mod_file WHERE filename=\"{file}\";")
                .ExecuteScalar();
            return res > 0;
        }

        public ModFile SaveModFile(ModFile mf)
        {
            return WithWriterLock(() =>
            {
                if (mf.Id == TempModFileId)
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
            });
        }

        /// <summary>
        /// 更新模组文件信息
        /// </summary>
        public void UpdateModFile(ModFile mf)
        {
            WithWriterLock(() =>
            {
                var command = Command(
                    $"UPDATE mod_file SET " +
                    $"filename = \"{mf.FilePath}\"" +
                    $",active = {(mf.Actived ? 1 : 0)} " +
                    $"WHERE id = {mf.Id};"
                );
                int effectRow = command.ExecuteNonQuery();
                if (effectRow == 0) throw new Exception("更新ModFile失败");
            });
        }

        /// <summary>
        /// [副作用]创建表格
        /// </summary>
        private void CreateTableIfNotExists()
        {
            var command = Command(
                "CREATE TABLE IF NOT EXISTS mod_file(" +
                "id INTEGER PRIMARY KEY" +
                ",filename TEXT NOT NULL UNIQUE" +
                ",active INT NOT NULL); "
            );
            command.ExecuteNonQuery();
        }

        private SQLiteCommand Command(string cmd)
        {
            var command = connection.CreateCommand();
            command.CommandText = cmd;
            return command;
        }

        /// <summary>
        /// 获取写入锁执行操作
        /// </summary>
        private void WithWriterLock(Action a)
        {
            rwlock.AcquireWriterLock(int.MaxValue);
            a();
            rwlock.ReleaseWriterLock();
        }

        /// <summary>
        /// 获取写入锁执行操作
        /// </summary>
        private T WithWriterLock<T>(Func<T> f)
        {
            rwlock.AcquireWriterLock(int.MaxValue);
            var res = f();
            rwlock.ReleaseWriterLock();
            return res;
        }

        /// <summary>
        /// 获取读取锁执行操作
        /// </summary>
        private void WithReaderLock(Action a)
        {
            rwlock.AcquireReaderLock(int.MaxValue);
            a();
            rwlock.ReleaseReaderLock();
        }

        private T WithReaderLock<T>(Func<T> f)
        {
            rwlock.AcquireReaderLock(int.MaxValue);
            var res = f();
            rwlock.ReleaseReaderLock();
            return res;
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
    }
}
