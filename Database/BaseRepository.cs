using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;

namespace studentManagementSyatem.Database
{
    public abstract class BaseRepository
    {
        protected SQLiteConnection GetConnection()
        {
            return DbConnection.GetConnection();
        }

        // INSERT, UPDATE, DELETE 
        protected int ExecuteNonQuery(string sql, SQLiteParameter[] parameters = null)
        {
            using (var conn = GetConnection())
            {
                conn.Open();
                using (var cmd = new SQLiteCommand(sql, conn))
                {
                    if (parameters != null)
                        cmd.Parameters.AddRange(parameters);
                    return cmd.ExecuteNonQuery();
                }
            }
        }

        // to return Single value  (COUNT etc.)
        protected object ExecuteScalar(string sql, SQLiteParameter[] parameters = null)
        {
            using (var conn = GetConnection())
            {
                conn.Open();
                using (var cmd = new SQLiteCommand(sql, conn))
                {
                    if (parameters != null)
                        cmd.Parameters.AddRange(parameters);
                    return cmd.ExecuteScalar();
                }
            }
        }

        // to return DataTable 
        protected DataTable ExecuteReader(string sql, SQLiteParameter[] parameters = null)
        {
            var dataTable = new DataTable();
            using (var conn = GetConnection())
            {
                conn.Open();
                using (var cmd = new SQLiteCommand(sql, conn))
                {
                    if (parameters != null)
                        cmd.Parameters.AddRange(parameters);
                    using (var adapter = new SQLiteDataAdapter(cmd))
                    {
                        adapter.Fill(dataTable);
                    }
                }
            }
            return dataTable;
        }

        // Return Single object  
        protected T ExecuteSingle<T>(string sql, Func<SQLiteDataReader, T> mapper,
            SQLiteParameter[] parameters = null)
        {
            using (var conn = GetConnection())
            {
                conn.Open();
                using (var cmd = new SQLiteCommand(sql, conn))
                {
                    if (parameters != null)
                        cmd.Parameters.AddRange(parameters);
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                            return mapper(reader);
                        return default(T);
                    }
                }
            }
        }

        // Return List of objects 
        protected List<T> ExecuteList<T>(string sql, Func<SQLiteDataReader, T> mapper,
            SQLiteParameter[] parameters = null)
        {
            var results = new List<T>();
            using (var conn = GetConnection())
            {
                conn.Open();
                using (var cmd = new SQLiteCommand(sql, conn))
                {
                    if (parameters != null)
                        cmd.Parameters.AddRange(parameters);
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                            results.Add(mapper(reader));
                    }
                }
            }
            return results;
        }

        // Helper: read string safely 
        protected string GetString(SQLiteDataReader reader, string col)
        {
            int i = reader.GetOrdinal(col);
            return reader.IsDBNull(i) ? string.Empty : reader.GetString(i);
        }

        // Helper: int safely read
        protected int GetInt(SQLiteDataReader reader, string col)
        {
            int i = reader.GetOrdinal(col);
            return reader.IsDBNull(i) ? 0 : reader.GetInt32(i);
        }

        // Last inserted row ID
        protected long GetLastInsertRowId()
        {
            object result = ExecuteScalar("SELECT last_insert_rowid();");
            return result != null ? Convert.ToInt64(result) : 0;
        }
    }
}