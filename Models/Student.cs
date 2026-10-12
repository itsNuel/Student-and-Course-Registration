namespace myproject.Models
{
    public class Student
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string Program { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string Phone { get; set; } = string.Empty;

        public int? CourseId { get; set; }

        public Course? Course { get; set; }

        public DateTime? EnrollmentDate { get; set; }
    }
}