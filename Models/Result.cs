namespace studentManagementSyatem.Models
{
    public class Result
    {
        public int Id { get; set; }
        public string StudentName { get; set; } = string.Empty;
        public string Subject { get; set; } = string.Empty;
        public string Marks { get; set; } = string.Empty;
        public string Grade { get; set; } = string.Empty;
    }
}