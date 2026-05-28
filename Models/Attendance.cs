namespace studentManagementSyatem.Models
{
    public class Attendance
    {
        public int Id { get; set; }
        public string StudentName { get; set; } = string.Empty;
        public string Date { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
    }
}