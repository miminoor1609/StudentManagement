using System.Collections.Generic;
using System.Data.SQLite;
using studentManagementSyatem.Models;

namespace studentManagementSyatem.Database
{
    public class CourseRepository : BaseRepository
    {
        public List<Course> GetAllCourses()
        {
            string sql = "SELECT Id, CourseName, Teacher, Duration FROM Courses ORDER BY CourseName";
            return ExecuteList(sql, reader => new Course
            {
                Id = GetInt(reader, "Id"),
                CourseName = GetString(reader, "CourseName"),
                Teacher = GetString(reader, "Teacher"),
                Duration = GetString(reader, "Duration")
            });
        }

        public bool AddCourse(Course course)
        {
            string sql = @"INSERT INTO Courses (CourseName, Teacher, Duration)
                           VALUES (@name, @teacher, @duration)";
            var parameters = new SQLiteParameter[]
            {
                new SQLiteParameter("@name", course.CourseName),
                new SQLiteParameter("@teacher", course.Teacher),
                new SQLiteParameter("@duration", course.Duration)
            };
            return ExecuteNonQuery(sql, parameters) > 0;
        }

        public bool UpdateCourse(Course course)
        {
            string sql = @"UPDATE Courses SET CourseName=@name, Teacher=@teacher,
                           Duration=@duration WHERE Id=@id";
            var parameters = new SQLiteParameter[]
            {
                new SQLiteParameter("@name", course.CourseName),
                new SQLiteParameter("@teacher", course.Teacher),
                new SQLiteParameter("@duration", course.Duration),
                new SQLiteParameter("@id", course.Id)
            };
            return ExecuteNonQuery(sql, parameters) > 0;
        }

        public bool DeleteCourse(int id)
        {
            string sql = "DELETE FROM Courses WHERE Id=@id";
            var parameters = new SQLiteParameter[]
            {
                new SQLiteParameter("@id", id)
            };
            return ExecuteNonQuery(sql, parameters) > 0;
        }

        public int GetCourseCount()
        {
            object result = ExecuteScalar("SELECT COUNT(*) FROM Courses");
            return result != null ? (int)(long)result : 0;
        }
    }
}