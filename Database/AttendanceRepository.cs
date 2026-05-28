using System.Collections.Generic;
using System.Data.SQLite;
using studentManagementSyatem.Models;

namespace studentManagementSyatem.Database
{
    public class AttendanceRepository : BaseRepository
    {
        public List<Attendance> GetAllAttendance()
        {
            string sql = "SELECT Id, StudentName, Date, Status FROM Attendance ORDER BY Date DESC";
            return ExecuteList(sql, reader => new Attendance
            {
                Id = GetInt(reader, "Id"),
                StudentName = GetString(reader, "StudentName"),
                Date = GetString(reader, "Date"),
                Status = GetString(reader, "Status")
            });
        }

        public bool AddAttendance(Attendance attendance)
        {
            string sql = @"INSERT INTO Attendance (StudentName, Date, Status)
                           VALUES (@student, @date, @status)";
            var parameters = new SQLiteParameter[]
            {
                new SQLiteParameter("@student", attendance.StudentName),
                new SQLiteParameter("@date", attendance.Date),
                new SQLiteParameter("@status", attendance.Status)
            };
            return ExecuteNonQuery(sql, parameters) > 0;
        }

        public bool UpdateAttendance(Attendance attendance)
        {
            string sql = @"UPDATE Attendance SET StudentName=@student, Date=@date,
                           Status=@status WHERE Id=@id";
            var parameters = new SQLiteParameter[]
            {
                new SQLiteParameter("@student", attendance.StudentName),
                new SQLiteParameter("@date", attendance.Date),
                new SQLiteParameter("@status", attendance.Status),
                new SQLiteParameter("@id", attendance.Id)
            };
            return ExecuteNonQuery(sql, parameters) > 0;
        }

        public bool DeleteAttendance(int id)
        {
            string sql = "DELETE FROM Attendance WHERE Id=@id";
            var parameters = new SQLiteParameter[]
            {
                new SQLiteParameter("@id", id)
            };
            return ExecuteNonQuery(sql, parameters) > 0;
        }
    }
}