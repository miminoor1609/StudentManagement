using System.Collections.Generic;
using System.Data.SQLite;
using studentManagementSyatem.Models;

namespace studentManagementSyatem.Database
{
    public class ResultRepository : BaseRepository
    {
        public List<Result> GetAllResults()
        {
            string sql = "SELECT Id, StudentName, Subject, Marks, Grade FROM Results ORDER BY StudentName";
            return ExecuteList(sql, reader => new Result
            {
                Id = GetInt(reader, "Id"),
                StudentName = GetString(reader, "StudentName"),
                Subject = GetString(reader, "Subject"),
                Marks = GetString(reader, "Marks"),
                Grade = GetString(reader, "Grade")
            });
        }

        public bool AddResult(Result result)
        {
            string sql = @"INSERT INTO Results (StudentName, Subject, Marks, Grade)
                           VALUES (@student, @subject, @marks, @grade)";
            var parameters = new SQLiteParameter[]
            {
                new SQLiteParameter("@student", result.StudentName),
                new SQLiteParameter("@subject", result.Subject),
                new SQLiteParameter("@marks", result.Marks),
                new SQLiteParameter("@grade", result.Grade)
            };
            return ExecuteNonQuery(sql, parameters) > 0;
        }

        public bool UpdateResult(Result result)
        {
            string sql = @"UPDATE Results SET StudentName=@student, Subject=@subject,
                           Marks=@marks, Grade=@grade WHERE Id=@id";
            var parameters = new SQLiteParameter[]
            {
                new SQLiteParameter("@student", result.StudentName),
                new SQLiteParameter("@subject", result.Subject),
                new SQLiteParameter("@marks", result.Marks),
                new SQLiteParameter("@grade", result.Grade),
                new SQLiteParameter("@id", result.Id)
            };
            return ExecuteNonQuery(sql, parameters) > 0;
        }

        public bool DeleteResult(int id)
        {
            string sql = "DELETE FROM Results WHERE Id=@id";
            var parameters = new SQLiteParameter[]
            {
                new SQLiteParameter("@id", id)
            };
            return ExecuteNonQuery(sql, parameters) > 0;
        }
    }
}