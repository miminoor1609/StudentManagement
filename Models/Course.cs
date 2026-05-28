namespace studentManagementSyatem.Models
{
    public class Course
    {
        public int Id { get; set; }
        public string CourseName { get; set; } = string.Empty;
        public string Teacher { get; set; } = string.Empty;
        public string Duration { get; set; } = string.Empty;
    }
}