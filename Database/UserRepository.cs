using System.Data.SQLite;
using studentManagementSyatem.Models;
using BCrypt.Net;

namespace studentManagementSyatem.Database
{
    using BCrypt = BCrypt.Net.BCrypt;

    public class UserRepository : BaseRepository
    {
        // Login verify  — thorugh  database 
        public User Authenticate(string username, string password)
        {
            string sql = @"SELECT UserId, Username, PasswordHash, FullName, UserType, IsActive
                           FROM Users WHERE Username = @username AND IsActive = 1";

            var parameters = new SQLiteParameter[]
            {
                new SQLiteParameter("@username", username)
            };

            var user = ExecuteSingle(sql, reader => new User
            {
                UserId = GetInt(reader, "UserId"),
                Username = GetString(reader, "Username"),
                PasswordHash = GetString(reader, "PasswordHash"),
                FullName = GetString(reader, "FullName"),
                UserType = GetString(reader, "UserType"),
                IsActive = GetInt(reader, "IsActive") == 1
            }, parameters);

            // BCrypt se password verification
            if (user != null && BCrypt.Verify(password, user.PasswordHash))
                return user;

            return null;
        }

        // Change Password
        public bool ChangePassword(int userId, string currentPassword, string newPassword)
        {
            string sql = "SELECT PasswordHash FROM Users WHERE UserId = @id";
            var p = new SQLiteParameter[] { new SQLiteParameter("@id", userId) };

            string currentHash = (string)ExecuteScalar(sql, p);
            if (currentHash == null || !BCrypt.Verify(currentPassword, currentHash))
                return false;

            string newHash = BCrypt.HashPassword(newPassword, workFactor: 11);
            string updateSql = "UPDATE Users SET PasswordHash = @hash WHERE UserId = @id";
            var updateParams = new SQLiteParameter[]
            {
                new SQLiteParameter("@hash", newHash),
                new SQLiteParameter("@id", userId)
            };
            return ExecuteNonQuery(updateSql, updateParams) > 0;
        }
    }
}