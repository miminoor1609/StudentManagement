using System.Collections.Generic;
using System.Data.SQLite;
using studentManagementSyatem.Models;

namespace studentManagementSyatem.Database
{
    public class StudentRepository : BaseRepository
    {
        public List<Student> GetAllStudents()
        {
            string sql = "SELECT Id, StudentName, RollNumber, ClassName, Phone FROM Students ORDER BY StudentName";
            return ExecuteList(sql, reader => new Student
            {
                Id = GetInt(reader, "Id"),
                StudentName = GetString(reader, "StudentName"),
                RollNumber = GetString(reader, "RollNumber"),
                ClassName = GetString(reader, "ClassName"),
                Phone = GetString(reader, "Phone")
            });
        }

        public bool AddStudent(Student student)
        {
            string sql = @"INSERT INTO Students (StudentName, RollNumber, ClassName, Phone)
                           VALUES (@name, @roll, @class, @phone)";
            var parameters = new SQLiteParameter[]
            {
                new SQLiteParameter("@name", student.StudentName),
                new SQLiteParameter("@roll", student.RollNumber),
                new SQLiteParameter("@class", student.ClassName),
                new SQLiteParameter("@phone", student.Phone)
            };
            return ExecuteNonQuery(sql, parameters) > 0;
        }

        public bool UpdateStudent(Student student)
        {
            string sql = @"UPDATE Students SET StudentName=@name, RollNumber=@roll,
                           ClassName=@class, Phone=@phone WHERE Id=@id";
            var parameters = new SQLiteParameter[]
            {
                new SQLiteParameter("@name", student.StudentName),
                new SQLiteParameter("@roll", student.RollNumber),
                new SQLiteParameter("@class", student.ClassName),
                new SQLiteParameter("@phone", student.Phone),
                new SQLiteParameter("@id", student.Id)
            };
            return ExecuteNonQuery(sql, parameters) > 0;
        }

        public bool DeleteStudent(int id)
        {
            string sql = "DELETE FROM Students WHERE Id=@id";
            var parameters = new SQLiteParameter[]
            {
                new SQLiteParameter("@id", id)
            };
            return ExecuteNonQuery(sql, parameters) > 0;
        }

        public int GetStudentCount()
        {
            object result = ExecuteScalar("SELECT COUNT(*) FROM Students");
            return result != null ? (int)(long)result : 0;
        }
    }
}