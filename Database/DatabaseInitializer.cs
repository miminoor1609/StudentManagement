using System;
using System.Data.SQLite;
using System.IO;

namespace studentManagementSyatem.Database
{
    public static class DatabaseInitializer
    {
        public static void Initialize()
        {
            // Create Database folder if exit not work
            string dbFolder = Path.GetDirectoryName(DbConnection.DatabasePath);
            if (!Directory.Exists(dbFolder))
                Directory.CreateDirectory(dbFolder);

            // Create Database file if exit not work
            if (!File.Exists(DbConnection.DatabasePath))
                SQLiteConnection.CreateFile(DbConnection.DatabasePath);

            // Create all Tables
            CreateTables();

            // Create Default admin user 
            CreateDefaultAdmin();
        }

        private static void CreateTables()
        {
            using (var conn = DbConnection.GetConnection())
            {
                conn.Open();

                string[] queries = {
                    // Students table
                    @"CREATE TABLE IF NOT EXISTS Students (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        StudentName TEXT NOT NULL,
                        RollNumber TEXT NOT NULL,
                        ClassName TEXT NOT NULL,
                        Phone TEXT NOT NULL
                    )",

                    // Courses table
                    @"CREATE TABLE IF NOT EXISTS Courses (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        CourseName TEXT NOT NULL,
                        Teacher TEXT NOT NULL,
                        Duration TEXT NOT NULL
                    )",

                    // Attendance table
                    @"CREATE TABLE IF NOT EXISTS Attendance (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        StudentName TEXT NOT NULL,
                        Date TEXT NOT NULL,
                        Status TEXT NOT NULL
                    )",

                    // Results table
                    @"CREATE TABLE IF NOT EXISTS Results (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        StudentName TEXT NOT NULL,
                        Subject TEXT NOT NULL,
                        Marks TEXT NOT NULL,
                        Grade TEXT NOT NULL
                    )",

                    // Users table (login ke liye)
                    @"CREATE TABLE IF NOT EXISTS Users (
                        UserId INTEGER PRIMARY KEY AUTOINCREMENT,
                        Username TEXT NOT NULL UNIQUE,
                        PasswordHash TEXT NOT NULL,
                        FullName TEXT NOT NULL,
                        UserType TEXT NOT NULL DEFAULT 'user',
                        IsActive INTEGER NOT NULL DEFAULT 1,
                        CreatedDate TEXT NOT NULL
                    )"
                };

                foreach (var query in queries)
                {
                    using (var cmd = new SQLiteCommand(query, conn))
                        cmd.ExecuteNonQuery();
                }
            }
        }

        private static void CreateDefaultAdmin()
        {
            // Check that admin exist or not
            using (var conn = DbConnection.GetConnection())
            {
                conn.Open();
                string checkSql = "SELECT COUNT(*) FROM Users WHERE Username = 'admin'";
                using (var cmd = new SQLiteCommand(checkSql, conn))
                {
                    long count = (long)cmd.ExecuteScalar();
                    if (count > 0) return; // Admin already exist
                }

                // Default admin  — Password: "admin123"
                string passwordHash = BCrypt.Net.BCrypt.HashPassword("admin123", workFactor: 11);

                string insertSql = @"INSERT INTO Users (Username, PasswordHash, FullName, UserType, IsActive, CreatedDate)
                                     VALUES ('admin', @hash, 'Administrator', 'admin', 1, @date)";

                using (var cmd = new SQLiteCommand(insertSql, conn))
                {
                    cmd.Parameters.AddWithValue("@hash", passwordHash);
                    cmd.Parameters.AddWithValue("@date", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}